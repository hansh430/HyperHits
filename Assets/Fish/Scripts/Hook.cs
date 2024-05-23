using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private Transform hookTransform;
    private Camera mainCamera;
    private Collider2D collider;
    private int length;
    private int strength;
    private int fishCount;
    private bool canMove;
    private List<Fish>hookedFishList;
    private Tweener cameraTween;

    private void Awake()
    {
        mainCamera = Camera.main;
        collider = GetComponent<Collider2D>();
        hookedFishList = new List<Fish>();
    }
   
    void Update()
    {
        if(canMove && Input.GetMouseButton(0))
        {
            Vector3 mousePos= mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position=transform.position;
            position.x = mousePos.x;
            transform.position = position;
        }     
    }
    public void StartFishing()
    {
        length = IdleManager.Instance.Length - 20;
        strength = IdleManager.Instance.Strength;
        fishCount = 0;

        float time = (-length) * 0.1f;
        cameraTween = mainCamera.transform.DOMoveY(length, 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y <= -11)
                transform.SetParent(mainCamera.transform);
        }).OnComplete(delegate
        {
            collider.enabled = true;
            cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (mainCamera.transform.position.y >= 25f)
                    stopFishing();
            });
        });
        FishUIManager.Instance.ChangeScreen(Menus.GAME);
        collider.enabled = false;
        canMove = true;
        hookedFishList.Clear();
    }

    private void stopFishing()
    {
        canMove=false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            collider.enabled = true;
            int fishPrice = 0;
            for(int i=0; i<hookedFishList.Count; i++)
            {
                hookedFishList[i].transform.SetParent(null) ;
                hookedFishList[i].ResetFish();
                fishPrice += hookedFishList[i].FishType.Price;
            }
            IdleManager.Instance.TotalGain = fishPrice;
            FishUIManager.Instance.ChangeScreen(Menus.END);
        });
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Fish")&& fishCount != strength)
        {
            fishCount++;
            Fish fish= target.GetComponent<Fish>();
            fish.Hooked();
            hookedFishList.Add(fish);
            target.transform.SetParent(transform);
            target.transform.position=hookTransform.position;
            target.transform.rotation = hookTransform.rotation;
            target.transform.localScale = Vector3.one;

            target.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity;
            });
            if (fishCount == strength)
                stopFishing();
        }
    }
}

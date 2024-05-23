using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private FishData fishType;
    private CircleCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private float screenLeftPosition;
    private Tweener tweener;

    public FishData FishType
    {
        get { return fishType; }
        set
        {
            fishType = value;
            collider.radius = fishType.colliderRadius;
            spriteRenderer.sprite = fishType.FishSprite;
        }
    }

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        screenLeftPosition = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }
    public void ResetFish()
    {
        if (tweener != null)
            tweener.Kill(false);

        float fishRandomPositionValue = UnityEngine.Random.Range(fishType.MinLength, fishType.MaxLength);
        collider.enabled = true;

        Vector3 fishCurrentPostion = transform.position;
        fishCurrentPostion.y = fishRandomPositionValue;
        fishCurrentPostion.x = screenLeftPosition;
        transform.position = fishCurrentPostion;

        float y = UnityEngine.Random.Range(fishRandomPositionValue - 1, fishRandomPositionValue + 1);
        Vector2 newPosition = new Vector2(-fishCurrentPostion.x, y);

        float delay = UnityEngine.Random.Range(0, 6);

        tweener = transform.DOMove(newPosition, 3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });
    }
    public void Hooked()
    {
        collider.enabled = false;
        tweener.Kill(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private float[] rotations = { 0, 90, 180, 270 };
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private float[] correctRotation;
    private int possibleRotation = 1;
    private PipeGameManager gameManager;
    private void Awake()
    {
        gameManager=FindObjectOfType<PipeGameManager>();
    }

    private void Start()
    {
        possibleRotation = correctRotation.Length;
        int randomNum = Random.Range(0, rotations.Length);
        transform.localEulerAngles = new Vector3(0, 0, rotations[randomNum]);
        if (possibleRotation > 1)
        {
            if (Mathf.Floor(transform.localEulerAngles.z) == correctRotation[0] || Mathf.Floor(transform.localEulerAngles.z) == correctRotation[1])
            {
                isPlaced = true;
                gameManager.CorrectPlacedPipe();
            }
        }
        else
        {
            if (Mathf.Floor(transform.localEulerAngles.z) == correctRotation[0])
            {
                isPlaced = true;
                gameManager.CorrectPlacedPipe();
            }
        }

    }
    private void OnMouseDown()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        if (possibleRotation > 1)
        {
            if (Mathf.Floor( transform.localEulerAngles.z) == correctRotation[0] || Mathf.Floor(transform.localEulerAngles.z) == correctRotation[1] && isPlaced == false)
            {
                isPlaced = true;
                gameManager.CorrectPlacedPipe();
            }
            else if (isPlaced == true)
            {
                isPlaced = false;
                gameManager.WrongPlacedPipe();
            }
        }
        else
        {
            if (Mathf.Floor(transform.localEulerAngles.z) == correctRotation[0] && isPlaced == false)
            {
                isPlaced = true;
                gameManager.CorrectPlacedPipe();
            }
            else if (isPlaced == true)
            {
                isPlaced = false;
                gameManager.WrongPlacedPipe();
            }
        }
       
    }
   
}

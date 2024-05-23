using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "FishData")]
public class FishData:ScriptableObject
{
    public int Price;
    public float FishCount;
    public float MinLength;
    public float MaxLength;
    public float colliderRadius;
    public Sprite FishSprite;
}

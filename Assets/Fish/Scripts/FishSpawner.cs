using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Fish fishPrefab;
    [SerializeField] private FishData[] fishTypes;
    private void Awake()
    {
        for(int i=0; i<fishTypes.Length; i++)
        {
            int num = 0;
            while (num < fishTypes[i].FishCount)
            {
                Fish fish = Instantiate<Fish>(fishPrefab);
                fish.FishType = fishTypes[i];
                fish.ResetFish();
                num++;
            }
        }
    }
}

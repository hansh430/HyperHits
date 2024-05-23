using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    [HideInInspector] public int Length;
    [HideInInspector] public int Strength;
    [HideInInspector] public int OfflineEarning;
    [HideInInspector] public int LenghtCost;
    [HideInInspector] public int StrengthCost;
    [HideInInspector] public int OfflineEarningCost;
    [HideInInspector] public int Wallet;
    [HideInInspector] public int TotalGain;

    private int[] costs = new int[]
    {
        120,151,197,250,324,414,537,687,892,1145,
        1484,1911,2479,3196,4148,5359,6954,9000,11687
    };

    public static IdleManager Instance;

    private void Awake()
    {
        if (IdleManager.Instance)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        else
        {
            IdleManager.Instance = this;
        }

        Length = -PlayerPrefs.GetInt("Length", 30);
        Strength = PlayerPrefs.GetInt("Strength", 3);
        OfflineEarning = PlayerPrefs.GetInt("Offline", 3);
        LenghtCost = costs[-Length / 10 - 3];
        StrengthCost = costs[Strength - 3];
        OfflineEarningCost = costs[OfflineEarning - 3];
        Wallet = PlayerPrefs.GetInt("Wallet", 100);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            print(now.ToString());
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date", string.Empty);
            if (@string != string.Empty)
            {
                DateTime dateTime = DateTime.Parse(@string);
                TotalGain = (int)((DateTime.Now - dateTime).TotalMinutes*OfflineEarning+1.0);
                FishUIManager.Instance.ChangeScreen(Menus.RETURN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }
    public void BuyLength()
    {
        Length -= 10;
        Wallet -= LenghtCost;
        LenghtCost = costs[-Length / 10 - 3];
        PlayerPrefs.SetInt("Length", -Length);
        PlayerPrefs.SetInt("Wallet", Wallet);
        FishUIManager.Instance.ChangeScreen(Menus.MAIN);
    }
    public void BuyStrength()
    {
        Strength++;
        Wallet -= StrengthCost;
        StrengthCost = costs[Strength - 3];
        PlayerPrefs.SetInt("Strength", Strength);
        PlayerPrefs.SetInt("Wallet", Wallet);
        FishUIManager.Instance.ChangeScreen(Menus.MAIN);
    }
    public void BuyOfflineEarnings()
    {
        OfflineEarning++;
        Wallet -= OfflineEarningCost;
        StrengthCost = costs[OfflineEarning - 3];
        PlayerPrefs.SetInt("Offline", OfflineEarning);
        PlayerPrefs.SetInt("Wallet", Wallet);
        FishUIManager.Instance.ChangeScreen(Menus.MAIN);
    }
    public void CollectMoney()
    {
        Wallet += TotalGain;
        PlayerPrefs.SetInt("Wallet", Wallet);
        FishUIManager.Instance.ChangeScreen(Menus.MAIN);
    }
    public void CollectDoubleMoney()
    {
        Wallet += TotalGain*2;
        PlayerPrefs.SetInt("Wallet", Wallet);
        FishUIManager.Instance.ChangeScreen(Menus.MAIN);
    }
}

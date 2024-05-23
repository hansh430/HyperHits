using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishUIManager : MonoBehaviour
{
    public static FishUIManager Instance;
    [Header("All Screens")]
    private GameObject CurrentScrreen;
    public GameObject GameScrreen;
    public GameObject MainScrreen;
    public GameObject EndScrreen;
    public GameObject ReturnScrreen;

    [Header("All Buttons")]
    public Button LengthButton;
    public Button StrengthButton;
    public Button OfflineButton;

    [Header("All Texts")]
    public TMP_Text GameScreenMoneyText;
    public TMP_Text LengthCostText;
    public TMP_Text LengthValueText;
    public TMP_Text StrengthCostText;
    public TMP_Text StrengthValueText;
    public TMP_Text OfflineCostText;
    public TMP_Text OfflineValueText;
    public TMP_Text EndScreenMoneyText;
    public TMP_Text ReturnScreenMoneyText;

    private int gameCount;
    private void Awake()
    {
        if (FishUIManager.Instance)
        {
            Destroy(base.gameObject);
        }
        else
            FishUIManager.Instance = this;

        CurrentScrreen = MainScrreen;
    }
    private void Start()
    {
        CheckIdles();
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        GameScreenMoneyText.text = "$" + IdleManager.Instance.Wallet;
        LengthCostText.text = "$" + IdleManager.Instance.LenghtCost;
        LengthValueText.text = -IdleManager.Instance.Length + "m";
        StrengthCostText.text="$"+IdleManager.Instance.StrengthCost;
        StrengthValueText.text = IdleManager.Instance.Strength + " fishes.";
        OfflineCostText.text = "$" + IdleManager.Instance.OfflineEarningCost;
        OfflineValueText.text = "$" + IdleManager.Instance.OfflineEarning + "/min";
    }

    private void CheckIdles()
    {
        int lenghtCost = IdleManager.Instance.LenghtCost;
        int strengthCost = IdleManager.Instance.StrengthCost;
        int offlineEarningsCosts = IdleManager.Instance.OfflineEarningCost;
        int wallet = IdleManager.Instance.Wallet;

        if (wallet < lenghtCost)
            LengthButton.interactable = false;
        else
            LengthButton.interactable = true;

        if(wallet< strengthCost)
            StrengthButton.interactable = false;
        else
            StrengthButton.interactable = true;

        if(wallet< offlineEarningsCosts)
            OfflineButton.interactable = false;
        else
            OfflineButton.interactable = true;
    }
    public void ChangeScreen(Menus menu)
    {
        CurrentScrreen.SetActive(false);

        switch (menu)
        {
            case Menus.MAIN:
                CurrentScrreen = MainScrreen;
                UpdateTexts();
                CheckIdles();
                break;

            case Menus.GAME:
                CurrentScrreen = GameScrreen;
                gameCount++;
                break;

            case Menus.END:
                CurrentScrreen = EndScrreen;
                SetEndScreenMoney();
                break;

            case Menus.RETURN:
                CurrentScrreen = ReturnScrreen;
                SetReturnScreenMoney();
                break;
        }
        CurrentScrreen.SetActive(true);
    }

    private void SetReturnScreenMoney()
    {
        ReturnScreenMoneyText.text = "$" + IdleManager.Instance.TotalGain+" gained while waiting!";
    }

    private void SetEndScreenMoney()
    {
        EndScreenMoneyText.text = "$" + IdleManager.Instance.TotalGain;
    }
}

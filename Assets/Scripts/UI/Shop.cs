using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private const string ShotgunSaveKey = "ShotgunDontDrope";
    private const string RifleSaveKey = "RifleDontDrope";
    private const string GunReadyName = "GunReady";

    [SerializeField] private GameObject _panel;
    [SerializeField] private RevardedMoneyHolder _revardedMoneyHolder;
    [SerializeField] private int _rifleCost;
    [SerializeField] private int _shotgunCost;
    [SerializeField] private Bonuses.BonusReward _bonusReward;

    private void Start()
    {
        PlayerPrefs.SetInt(RifleSaveKey, 0);
        PlayerPrefs.SetInt(ShotgunSaveKey, 0);
        PlayerPrefs.Save();
    }

    public void OpenShop()
    {
        _panel.SetActive(true);
    }

    public void CloseShop()
    {
        _panel.SetActive(false);
    }

    public void BuyRifle()
    {
        if(_revardedMoneyHolder.Money >= _rifleCost)
        {
            PlayerPrefs.SetInt(RifleSaveKey, 1);
            PlayerPrefs.Save();
            _revardedMoneyHolder.TakeMoney(_rifleCost);
            _bonusReward.PrepareBonus(GunReadyName, 1);
        }     
    }

    public void BuyShotgun()
    {
        if(_revardedMoneyHolder.Money >= _shotgunCost)
        {
            PlayerPrefs.SetInt(ShotgunSaveKey, 1);
            PlayerPrefs.Save();
            _revardedMoneyHolder.TakeMoney(_shotgunCost);
            _bonusReward.PrepareBonus(GunReadyName, 2);
        }
    }
}

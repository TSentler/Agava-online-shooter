using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevardedMoneyHolder : MonoBehaviour
{
    private const string SaveKey = "RevardedMoney";

    [SerializeField] private int _givedMoney;
    [SerializeField] private RevardedVideo _revardedVideo;

    private int _money;

    public int Money => _money;

    public Action<int> MoneyChanged; 

    private void Awake()
    {
        _money = PlayerPrefs.GetInt(SaveKey, 0);
    }

    public void ShowRevarded()
    {
        _revardedVideo.OnRevardedVideoButtonClick("Money");
    }

    public void GiveMoney(int count)
    {
        _money += count;
        MoneyChanged?.Invoke(Money);
    }

    public void TakeMoney(int count)
    {
        _money -= count;
        MoneyChanged?.Invoke(Money);
    }
}

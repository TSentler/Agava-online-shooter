using Bonuses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevardedVideo : MonoBehaviour
{
    [SerializeField] private float _extraHP;

    private readonly string _increaseHPName = "IncreaseHP",
         _gunReadyName = "GunReady";

    private bool _isRewarded;
    private BonusReward _bonusReward;
    private string _name;

    private Action _adOpened;
    private Action _adRewarded;
    private Action _adClosed;
    private Action<string> _adErrorOccured;
    private Action _adErrorVK;
    //private CrazyAds.AdBreakCompletedCallback AdBreakCompletedCallback;
    //private CrazyAds.AdErrorCallback AdErrorCallback;

    public bool IsRewarded => _isRewarded;

    private void Awake()
    {
        _bonusReward = FindObjectOfType<BonusReward>();
    }

    private void OnEnable()
    {
        _adOpened += OnAdOpened;
        _adRewarded += OnAdRewarded;
        _adClosed += OnAdClosed;
        _adErrorOccured += OnAdErrorOccured;
        //AdBreakCompletedCallback += OnCrazyGamesRevardedAd;
        //AdErrorCallback += OnCrazyGamesErrorAd;
        _adErrorVK += OnAdErrorVk;
        _isRewarded = false;
    }

    private void OnDisable()
    {
        _adOpened -= OnAdOpened;
        _adRewarded -= OnAdRewarded;
        _adClosed -= OnAdClosed;
        _adErrorOccured -= OnAdErrorOccured;
        //AdBreakCompletedCallback -= OnCrazyGamesRevardedAd;
        //AdErrorCallback -= OnCrazyGamesErrorAd;
        _adErrorVK -= OnAdErrorVk;
    }

    public void OnRevardedVideoButtonClick(string name)
    {
        _name = name;
#if YANDEX_GAMES
        Agava.YandexGames.VideoAd.Show(_adOpened, _adRewarded, _adClosed, _adErrorOccured);
#endif
#if VK_GAMES
        Agava.VKGames.VideoAd.Show(_adRewarded,_adErrorVK);
#endif
#if CRAZY_GAMES
        CrazyAds.Instance.beginAdBreakRewarded(AdBreakCompletedCallback, AdErrorCallback);
#endif
       
    }

    private void OnAdErrorOccured(string obj)
    {
        _isRewarded = false;
    }

    private void OnAdClosed()
    {

    }

    private void OnAdRewarded()
    {
        if (_name == "Rifle")
        {
            _bonusReward.PrepareBonus(_gunReadyName, 1);
        }
        else if (_name == "HP")
        {
            _bonusReward.PrepareBonus(_increaseHPName, _extraHP);
        }
        else if (_name == "Shorgun")
        {
            _bonusReward.PrepareBonus(_gunReadyName, 2);
        }

        _isRewarded = true;
    }

    private void OnAdOpened()
    {

    }

    private void OnCrazyGamesRevardedAd()
    {
        _isRewarded = true;
    }

    private void OnCrazyGamesErrorAd()
    {
        _isRewarded = false;
    }

    private void OnAdErrorVk()
    {
        _isRewarded = false;
    }
}

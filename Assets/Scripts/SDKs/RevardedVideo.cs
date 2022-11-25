using Bonuses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevardedVideo : MonoBehaviour
{
    [SerializeField] private float _extraHP;
    [SerializeField] private AnalitickEventSender _analitickEvenSender;
    [SerializeField] private RevardedMoneyHolder _moneyHolder;
    [SerializeField] private CanvasGroup _rewardWindow;
    [SerializeField] private Image _rifleButton;
    [SerializeField] private Image _shotgunButton;
    [SerializeField] private Image _hpButton;
    [SerializeField] private Sprite _graySprite;

    private readonly string _increaseHPName = "IncreaseHP",
         _gunReadyName = "GunReady";

    private bool _isRewarded;
    private BonusReward _bonusReward;
    private string _name;
    private float _speedAlpha = 2f;

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
        _analitickEvenSender = FindObjectOfType<AnalitickEventSender>();
        _rewardWindow.alpha = 0;
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
        AudioListener.pause = false;
    }

    private void OnAdRewarded()
    {
        if (_name == "Rifle")
        {
            _bonusReward.PrepareBonus(_gunReadyName, 1);
            _analitickEvenSender.OnRifleRevardWasShow();
            _rifleButton.sprite = _graySprite;
            StartCoroutine(ShowRewardWindow());
        }
        else if (_name == "HP")
        {
            _bonusReward.PrepareBonus(_increaseHPName, _extraHP);
            _analitickEvenSender.OnHpRevardWasShown();
            _hpButton.sprite = _graySprite;
            StartCoroutine(ShowRewardWindow());
        }
        else if (_name == "Shotgun")
        {
            _bonusReward.PrepareBonus(_gunReadyName, 2);
            _analitickEvenSender.OnShotgunRevardWasShow();
            _shotgunButton.sprite = _graySprite;
            StartCoroutine(ShowRewardWindow());
        }
        else if (_name == "Money")
        {
            _moneyHolder.GiveMoney(100);
        }
        _isRewarded = true;
     
    }

    private void OnAdOpened()
    {
        AudioListener.pause = true;
    }

    private void OnCrazyGamesRevardedAd()
    {
        if (_name == "Rifle")
        {
            _bonusReward.PrepareBonus(_gunReadyName, 1);
            //_analitickEvenSender.OnRifleRevardWasShow();
            _rifleButton.sprite = _graySprite;
            StartCoroutine(ShowRewardWindow());
        }
        else if (_name == "HP")
        {
            _bonusReward.PrepareBonus(_increaseHPName, _extraHP);
            //_analitickEvenSender.OnHpRevardWasShown();
            _hpButton.sprite = _graySprite;
            StartCoroutine(ShowRewardWindow());
        }
        else if (_name == "Shotgun")
        {
            _bonusReward.PrepareBonus(_gunReadyName, 2);
            //_analitickEvenSender.OnShotgunRevardWasShow();
            _shotgunButton.sprite = _graySprite;
            StartCoroutine(ShowRewardWindow());
        }
        else if (_name == "Money")
        {
            _moneyHolder.GiveMoney(100);
        }

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

    private IEnumerator ShowRewardWindow()
    {
        _rewardWindow.alpha = 1;
        yield return new WaitForSeconds(5f);

        while(_rewardWindow.alpha != 0)
        {
            _rewardWindow.alpha = Mathf.MoveTowards(_rewardWindow.alpha, 0, Time.deltaTime * _speedAlpha);
            yield return null;
        }
    }
}

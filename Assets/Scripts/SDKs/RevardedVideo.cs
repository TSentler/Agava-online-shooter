using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevardedVideo : MonoBehaviour
{
    private Action _adOpened;
    private Action _adRewarded;
    private Action _adClosed;
    private Action<string> _adErrorOccured;

    private void OnEnable()
    {
        _adOpened += OnAdOpened;
        _adRewarded += OnAdRewarded;
        _adClosed += OnAdClosed;
        _adErrorOccured += OnAdErrorOccured;
    }

    private void OnDisable()
    {
        _adOpened -= OnAdOpened;
        _adRewarded -= OnAdRewarded;
        _adClosed -= OnAdClosed;
        _adErrorOccured -= OnAdErrorOccured;
    }

    public void OnRevardedVideoButtonClick()
    {
#if YANDEX_GAMES
        VideoAd.Show(_adOpened, _adRewarded, _adClosed, _adErrorOccured);
#endif
#if VK_GAMES
        Agava.VKGames.VideoAd.Show(_adRewarded);
#endif
    }

    private void OnAdErrorOccured(string obj)
    {
      
    }

    private void OnAdClosed()
    {
      
    }

    private void OnAdRewarded()
    {
       
    }

    private void OnAdOpened()
    {

    }
}

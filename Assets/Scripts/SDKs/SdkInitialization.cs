using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SdkInitialization : MonoBehaviour
{
    private void Awake()
    {
#if !CRAZY_GAMES
        StartCoroutine(Init());
#endif
#if CRAZY_GAMES
        SceneManager.LoadScene(1);
#endif
    }

    private IEnumerator Init()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
       yield return new WaitForSeconds(0.1f);
#elif YANDEX_GAMES
        while(Agava.YandexGames.YandexGamesSdk.IsInitialized == false)
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
        }
#elif VK_GAMES
        while (Agava.VKGames.VKGamesSdk.Initialized == false)
        {     
            yield return Agava.VKGames.VKGamesSdk.Initialize();
        }

        //GameAnalyticsSDK.GameAnalytics.Initialize();
        //Analitic.StartGame(_countOfStart);
        //_countOfStart++;
        //PlayerPrefs.SetInt("CountOfStart", _countOfStart);
#endif
        SceneManager.LoadScene(1);
    }
}

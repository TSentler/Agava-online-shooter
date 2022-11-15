using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SdkInitialization : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        
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
        yield break;
    }
}

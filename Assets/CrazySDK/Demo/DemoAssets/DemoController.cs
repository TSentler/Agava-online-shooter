using System.Collections;
using System.Collections.Generic;
using CrazyGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoController : MonoBehaviour
{
    public void ChangeDemoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToMainDemo()
    {
        SceneManager.LoadScene("MainDemo");
    }

    public void PrintUserInfo()
    {
        CrazySDK.Instance.GetUserInfo(userInfo =>
        {
            Debug.Log("Browser info: " + userInfo.browser.name + " " + userInfo.browser.version + ", OS info: " + userInfo.os.name + " " +
                      userInfo.os.version + ", Device type: " + userInfo.device.type);
        });
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalitickEventSender : MonoBehaviour
{
    [SerializeField] private Metrica _metrica;

    public void OnRifleRevardWasShow()
    {
        _metrica.OnSendButtonClick("rifleRevarded", "{ \"" + "rifleRevarded" + "\":\"" + "wasShown" + "\" }");
    }

    public void OnShotgunRevardWasShow()
    {
        _metrica.OnSendButtonClick("shotgunRevard", "{ \"" + "shotgunRevard" + "\":\"" + "wasShown" + "\" }");
    }

    public void OnHpRevardWasShown()
    {
        _metrica.OnSendButtonClick("hpRevard", "{ \"" + "hpRevard" + "\":\"" + "wasShown" + "\" }");
    }
}

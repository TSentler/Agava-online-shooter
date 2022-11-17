using Agava.YandexMetrica;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metrica : MonoBehaviour
{
    public void OnSendButtonClick(string eventName, string eventData)
    {
        YandexMetrica.Send(eventName,eventData);
    }
}

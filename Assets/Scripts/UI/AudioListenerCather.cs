using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerCather : MonoBehaviour
{
    private const string VolumeSaveKey = "Volume";

    [SerializeField] private PhotonView _photonView;

    private VolumeChanger _volumeChanger;

    private void Awake()
    {
        _volumeChanger = FindObjectOfType<VolumeChanger>();
        _volumeChanger.GetSavePlayerPrefs();
    }

    private void OnEnable()
    {
        _volumeChanger.OnValueChanged += ChangeVolume;
    }

    private void OnDisable()
    {
        _volumeChanger.OnValueChanged -= ChangeVolume;
    }

    private void ChangeVolume(float value)
    {
        if (_photonView.IsMine)
        {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat(VolumeSaveKey, value);
        }
    }
}

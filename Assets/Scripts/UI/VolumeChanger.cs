using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour
{
    private const string VolumeSaveKey = "Volume";

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Slider _slieder;
    [SerializeField] private float _standartVolume;

    public event UnityAction<float> OnValueChanged;

    private void Start()
    {

        GetSavePlayerPrefs();

    }

    private void OnEnable()
    {
        _slieder.onValueChanged.AddListener(OnChangeVolume);
    }

    private void OnDisable()
    {
        _slieder.onValueChanged.RemoveListener(OnChangeVolume);
    }

    public void GetSavePlayerPrefs()
    {
        if (PlayerPrefs.HasKey(VolumeSaveKey))
        {
            _slieder.value = PlayerPrefs.GetFloat(VolumeSaveKey);
            AudioListener.volume = _slieder.value;
        }
        else
        {
            _slieder.value = _standartVolume;
            AudioListener.volume = _standartVolume;
        }
    }

    private void OnChangeVolume(float value)
    {
        PlayerPrefs.SetFloat(VolumeSaveKey, _slieder.value);
        AudioListener.volume = value;
        Debug.Log(1);
        OnValueChanged?.Invoke(_slieder.value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimSensitivity : MonoBehaviour
{
    private const string MouseSensitivitySaveKey = "AimSensitivity";

    [SerializeField] private CanvasGroup _menu;
    [SerializeField] private Slider _slieder;
    [SerializeField] private float _standartSensetivity;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(MouseSensitivitySaveKey))
        {
            _slieder.value = PlayerPrefs.GetFloat(MouseSensitivitySaveKey);
        }
        else
        {
            _slieder.value = _standartSensetivity;
        }
    }

    private void OnEnable()
    {
        _slieder.onValueChanged.AddListener(ChangeSensetivity);
    }

    private void Start()
    {
        _menu.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _slieder.onValueChanged.RemoveListener(ChangeSensetivity);
    }

    private void ChangeSensetivity(float value)
    {
        PlayerPrefs.SetFloat(MouseSensitivitySaveKey, _slieder.value);
    }
}

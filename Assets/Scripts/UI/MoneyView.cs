using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{
    [SerializeField] private RevardedMoneyHolder _revardedMoneyHolder;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _revardedMoneyHolder.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _revardedMoneyHolder.MoneyChanged -= OnMoneyChanged;
    }

    private void Start()
    {
        _text.text = _revardedMoneyHolder.Money.ToString();
    }

    private void OnMoneyChanged(int obj)
    {
        _text.text = _revardedMoneyHolder.Money.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicatorText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _lifetime;

    private float _speed = 5f;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        float fraction = _lifetime / 2f;

        if (_timer > _lifetime)
        {
            Destroy(gameObject);
        }
        else if (_timer > fraction)
        {
            _text.color = Color.Lerp(_text.color, Color.clear, (_timer - fraction) / (_lifetime - fraction));
        }

        transform.position = Vector3.Lerp(transform.position, Vector3.up + transform.position, _speed);
    }

    public void SetDamageText(float damage)
    {
        _text.text = ((int)-damage).ToString();
    }
}
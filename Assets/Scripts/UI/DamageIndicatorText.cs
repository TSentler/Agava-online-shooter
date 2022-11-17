using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicatorText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _lifetime;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    private Vector3 _initPosition;
    private Vector3 _targetPosition;
    private float _timer;

    private void Start()
    {
        float direction = Random.rotation.eulerAngles.z;
        _initPosition = transform.position;
        float distance = Random.Range(_minDistance, _maxDistance);
        _targetPosition = _initPosition + (Quaternion.Euler(0, 0, direction) * new Vector3(distance, distance, 0f));
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        float fraction = _lifetime / 2f;

        if(_timer > _lifetime)
        {
            Destroy(gameObject);
        }
        else if(_timer > fraction)
        {
            _text.color = Color.Lerp(_text.color, Color.clear, (_timer - fraction) / (_lifetime - fraction));
        }

        transform.position = Vector3.Lerp(_initPosition, _targetPosition, Mathf.Sin(_timer / _lifetime));
        transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(_timer / _lifetime));
    }

    public void SetDamageText(float damage)
    {
        _text.text = damage.ToString();
    }
}

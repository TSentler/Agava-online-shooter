using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HealthText : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void SetHealth(float value)
    {
        _text.text = value.ToString();
    }
}

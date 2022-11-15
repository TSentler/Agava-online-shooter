using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorSetter : MonoBehaviour
{
    [SerializeField] private Color _activeColor;

    private Color _defaultColor = Color.white;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        SetDefaultColor();
    }

    public void SetActiveColor()
    {
        _image.color = _activeColor;
    }

    public  void SetDefaultColor()
    {
        _image.color = _defaultColor;
    }
}

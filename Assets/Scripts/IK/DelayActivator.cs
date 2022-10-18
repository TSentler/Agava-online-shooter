using UnityEngine;

public class DelayActivator : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _component;

    private void Start()
    {
        _component.enabled = true;
    }
}

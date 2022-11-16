using Agava.WebUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMuter : MonoBehaviour
{
    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        // Use both pause and volume muting methods at the same time.
        // They're both broken in Web, but work perfect together. Trust me on this.
        AudioListener.pause = inBackground;
        AudioListener.volume = inBackground ? 0f : 1f;
    }
}

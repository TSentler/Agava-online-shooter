using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Network.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetVersion(string version)
        {
            _text.SetText(version);
        }
    }
}

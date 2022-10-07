using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItemView : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _nameText;

    private Player _player;

    public void Render(Player player, string name)
    {
        _player = player;
        _nameText.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Destroy(gameObject);
    }
}

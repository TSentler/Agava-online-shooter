using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Social : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _button;

    private void Awake()
    {
#if !VK_GAMES
_button.SetActive(false);
#endif
    }

    public void OnSocialButtonClick()
    {
        _panel.SetActive(true);
    }

    public void OnCloseButtonClick()
    {
        _panel.SetActive(false);
    }

    public void OnJoinButtonCkick()
    {
        Agava.VKGames.Community.InviteToIJuniorGroup();
    }

    public void OnInviteButtonClick()
    {
        Agava.VKGames.SocialInteraction.InviteFriends();
    }
}

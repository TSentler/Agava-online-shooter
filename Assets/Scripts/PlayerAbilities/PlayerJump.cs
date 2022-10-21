using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(Animator),
        typeof(CharacterController))]
    public class PlayerJump : MonoBehaviour
    {
    }
}

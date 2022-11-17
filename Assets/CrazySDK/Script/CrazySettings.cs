using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrazyGames
{
    /**
    * Contains public data that can be modified by the game developers that use the SDK.
    */
    public class CrazySettings : ScriptableObject
    {
        /**
         * SiteLock doesn't block these domains, and the SDK is not initialized here. Used for the domains where game developers host their games.
         */
        [Tooltip("SiteLock doesn't block these domains. Here you can add the domains where your game is hosted.")]
        public string[] whitelistedDomains;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CrazyGames
{
    /**
     * Site locking is checked on game start.
     */
    public class SiteLock
    {
        private static string sitelockVersion = "1.4.1";

        private static readonly string[] allowedLocalHosts = {"localhost"};

        private static readonly string[] allowedRemoteHosts =
        {
            "gioca.re",
            "1001juegos.com",
            "speelspelletjes.nl",
            "onlinegame.co.id",
            "developer.unity3dusercontent.com" // unity cloud build URL
        };

        /// Do we permit execution from local host or local file system?
        private static readonly bool allowLocalHost = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
#if !UNITY_WEBGL
            return;
#endif

            if (IsOnWhitelistedDomain())
            {
                // if the game is running on dev's domain, don't proceed further with the sitelock
                return;
            }

            Debug.Log("[CrazySDK] SiteLock v" + sitelockVersion);

            var hasFullSDK = Type.GetType("CrazyGames.CrazySDK") != null;
            if (!hasFullSDK)
            {
                InitSDKLite(sitelockVersion);
            }

            var url = Application.absoluteURL;
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                //String is not a valid URL. 
#if UNITY_EDITOR
                //print("URL Not Valid");
                return;
#else
                Crash(0);
                return;
#endif
            }

            var host = uri.Host;

            var splittedHost = host.Split("."[0]);
            var crazyIndex = -1;

            for (var i = 0; i < splittedHost.Length; i++)
            {
                var split = splittedHost[i].ToLower();
                if (split != "crazygames" && split != "dev-crazygames") continue;
                crazyIndex = i;
                break;
            }


            if (crazyIndex >= 0 &&
                splittedHost.Length == crazyIndex + 2 ||
                splittedHost.Length == crazyIndex + 3 && splittedHost[crazyIndex + 1].Length <= 3
               )
            {
#if UNITY_EDITOR
                Debug.Log("SUCCESS!  We Are On CrazyGames server!  ");
#endif
                return; //no more logic needed 
            }

#if UNITY_EDITOR
            Debug.Log("FAILED!  We Are Not On CrazyGames Server!");
#endif
            //so, continue with checking other allowed domains ...

#if !UNITY_EDITOR
            if (!IsOnValidHost())
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Failed valid remote host test, Crashing");
                }
                Crash(0);
                return;
            }
#endif
        }

        private static bool IsOnValidHost()
        {
            return IsOnValidLocalHost() || IsOnValidRemoteHost();
        }

        /// Determine if the current host exists in the given list of permitted hosts.
        private static bool IsValidHost(string[] hosts)
        {
            if (Debug.isDebugBuild)
            {
                var msg = new StringBuilder();
                msg.Append("Checking against list of hosts: ");
                foreach (var url in hosts)
                {
                    msg.Append(url);
                    msg.Append(",");
                }

                Debug.Log(msg.ToString());
            }

            // check current host against each of the given hosts
            var hostRegex = new Regex(@"^(\w+)://(?<hostname>[^/]+?)(?<port>:\d+)?/");
            var match = hostRegex.Match(Application.absoluteURL);
            if (!match.Success)
                // somehow our current url is not a valid url
                return false;

            var hostname = match.Groups["hostname"].Value;
            var splittedHost = hostname.Split("."[0]);
            return hosts.Any(host => DoesHostMatch(host, splittedHost));
        }

        private static bool DoesHostMatch(string allowedHost, string[] applicationHost)
        {
            var splitAllowed = allowedHost.Split("."[0]);

            if (applicationHost.Length < splitAllowed.Length) return false;

            for (var i = 0; i < splitAllowed.Length; i++)
            {
                var currentSplit = splitAllowed[i];
                var currentHost = applicationHost[applicationHost.Length - splitAllowed.Length + i];
                if (!currentSplit.Equals(currentHost)) return false;
            }

            return true;
        }

        /// Determine if the current host is a valid local host.
        private static bool IsOnValidLocalHost()
        {
            return allowLocalHost && IsValidHost(allowedLocalHosts);
        }

        /// <summary>
        ///     Determine if the current host is a valid remote host.
        /// </summary>
        /// <returns>True if the game is permitted to execute from the remote host.</returns>
        private static bool IsOnValidRemoteHost()
        {
            return IsValidHost(allowedRemoteHosts);
        }

        // redirects can be prevented, so just enforce an infinite loop to crash unity
        private static void Crash(int i)
        {
            Crash(i++);
        }

        public static bool IsOnWhitelistedDomain()
        {
            var settings = Resources.Load<CrazySettings>("CrazyGamesSettings");
            var whitelistedDevDomains = new List<string>();
            if (settings == null || settings.whitelistedDomains == null) return false;

            foreach (var settingsDomain in settings.whitelistedDomains)
            {
                var domain = settingsDomain;

                if (!domain.StartsWith("http://") && !domain.StartsWith("https://"))
                {
                    domain = "http://" + domain;
                }

                try
                {
                    Uri uri = new Uri(domain);
                    whitelistedDevDomains.Add(uri.Host);
                }
                catch (Exception e)
                {
                    Debug.LogError("[CrazySDK] Failed to parse whitelisted domain: " + settingsDomain);
                }
            }

            return IsValidHost(whitelistedDevDomains.ToArray());
        }

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern bool InitSDKLite(string version);
#else
                private static bool InitSDKLite(string version)
                {
                    return true;
                }
#endif
    }
}
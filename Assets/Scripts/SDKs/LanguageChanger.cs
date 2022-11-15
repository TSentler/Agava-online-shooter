using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private LeanLocalization _leanLocalization;

    private void Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        
#elif YANDEX_GAMES

        switch (YandexGamesSdk.Environment.i18n.lang)
        {
            case "en":
                _leanLocalization.SetCurrentLanguage("English");
                break;
            case "tr":
                _leanLocalization.SetCurrentLanguage("Turkish");
                break;
            case "ru":
                _leanLocalization.SetCurrentLanguage("Russian");
                    break;
            default:
                _leanLocalization.SetCurrentLanguage("English");
                break;
        }
#elif VK_GAMES
        _leanLocalization.SetCurrentLanguage("Russian");
#endif
    }
}

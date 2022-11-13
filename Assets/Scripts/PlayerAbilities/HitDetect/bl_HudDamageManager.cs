using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class bl_HudDamageManager : MonoBehaviour {

    [Header("Settings")]
    [Range(0,10)]
    [SerializeField]private float DelayFade = 0.25f;
    [Range(0.01f,5)]
    [SerializeField]private float FadeSpeed = 0.4f;
    [Range(0.1f,0.9f)]
    [SerializeField]private float MinAlpha = 0.4f;
    [SerializeField]private AnimationCurve CurveFade;

    [Header("References")]
    [SerializeField]private CanvasGroup m_canvasGroup;

    private float Alpha = 0;
    private float NextDelay = 0;

    /// <summary>
    /// Register all callbacks 
    /// </summary>
    void OnEnable()
    {
        bl_DamageDelegate.OnDamageEvent += OnDamage;
    }

    /// <summary>
    /// UnRegister all callbacks 
    /// </summary>
    void OnDisable()
    {
        bl_DamageDelegate.OnDamageEvent -= OnDamage;
    }

    /// <summary>
    /// This is called by event delegate
    /// sure to call when player receive the damage.
    /// </summary>
    void OnDamage(bl_DamageInfo info)
    {

        //Calculate the diference in health for apply to the alpha.
        Alpha = MinAlpha;
        //Ensure that alpha is never less than the minimum allowed
        Alpha = Mathf.Clamp(Alpha, MinAlpha, 1);
        //Update delay
        NextDelay = Time.time + DelayFade;
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        //Apply fade effect to HUD.
        FadeRedScreen();
    }

    /// <summary>
    /// 
    /// </summary>
    void FadeRedScreen()
    {
        if (m_canvasGroup.alpha != Alpha)
        {
            if (Time.time > NextDelay && Alpha > 0)
            {
                Alpha = Mathf.Lerp(Alpha, 0, Time.deltaTime);
                Alpha = CurveFade.Evaluate(Alpha);
            }
            m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, Alpha, Time.deltaTime * FadeSpeed);
        }
    }

  
    /// <summary>
    /// Simple restart 
    /// this is not requiered to use in your project.
    /// </summary>
    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public float BloodFadeSpeed 
    {
        get
        {
           return FadeSpeed;
        }
        set
        {
            FadeSpeed = value;
        }
    }

    public float FadeDelay
    {
        get
        {
            return DelayFade;
        }
        set
        {
            DelayFade = value;
        }
    }
}
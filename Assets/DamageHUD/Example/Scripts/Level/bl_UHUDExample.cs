using UnityEngine;

public class bl_UHUDExample : MonoBehaviour {

    public bl_IndicatorManager Manager;
    public bl_HudDamageManager HUDManager;
    public Texture2D CursorExample = null;

    void Start()
    {
        Cursor.SetCursor(CursorExample,new Vector2(15f,15),CursorMode.Auto);
    }

    public void OnPIIChange(float value)
    {
        Manager.IndicatorIncline = value;
    }

    public void OnPISChange(float value)
    {
        Manager.PivotSize = value;
    }

    public void OnBFSChange(float value)
    {
        HUDManager.BloodFadeSpeed = value;
    }

    public void OnBFDChange(float value)
    {
        HUDManager.FadeDelay = value;
    }

    public void OnUseReferenceChange(bool value)
    {
        Manager.UseCameraReference = value;
    }

    public void OnShowDistanceChange(bool value)
    {
       Manager.ShowDistance = value;
    }
}
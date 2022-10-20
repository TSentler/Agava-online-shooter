using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeaveRoomButtonEnable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.visible = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.visible = false;
    }
}

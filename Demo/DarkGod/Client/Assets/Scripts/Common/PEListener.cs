using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public Action<PointerEventData> onclickDown;
    public Action<PointerEventData> onclickUp;
    public Action<PointerEventData> onDrag;
    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onclickDown!=null)
        {
            onclickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onclickUp != null)
        {
            onclickUp(eventData);
        }
    }

}

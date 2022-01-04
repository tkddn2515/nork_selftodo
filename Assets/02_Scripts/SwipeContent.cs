using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeContent : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }

    public void OnPointerClick()
    {
        Debug.Log("OnPointerClick");
    }

    public void OnBeginDrag()
    {
        Debug.Log("OnBeginDrag" + name);
    }

    public void OnDrag()
    {
        Debug.Log("OnDrag" + name);
    }

    public void OnEndDrag()
    {
        Debug.Log("OnEndDrag" + name);
    }
}

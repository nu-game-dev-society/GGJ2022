using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverAffector : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent hovered;
    public UnityEvent unhovered;
    public void OnPointerExit(PointerEventData eventData)
    {
        unhovered.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered.Invoke();
    }
}

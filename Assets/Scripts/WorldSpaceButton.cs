using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldSpaceButton : MonoBehaviour, IButtonEvents
{
    public UnityEvent hoverEnter;
    public UnityEvent hoverExit;
    public UnityEvent onClicked;
    public void OnClicked()
    {
        onClicked.Invoke();
    }

    public void OnHoverEnter()
    {
        hoverEnter.Invoke();
    }

    public void OnHoverExit()
    {
        hoverExit.Invoke();
    }
}

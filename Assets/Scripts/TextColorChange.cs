using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextColorChange : MonoBehaviour
{
    TextMeshPro text;
    public Color highlightColor, defaultColor;
    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }
    public void HighlightColor()
    {
        text.color = highlightColor;
    }
    public void DefaultColor()
    {
        text.color = defaultColor;
    }
}

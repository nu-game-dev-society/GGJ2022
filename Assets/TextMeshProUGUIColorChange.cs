using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProUGUIColorChange : MonoBehaviour
{
    TextMeshProUGUI text;
    public Color highlightColor, defaultColor;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
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

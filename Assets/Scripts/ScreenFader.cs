using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public float value = 0.0f;
    public bool toBlack = false;

    public static ScreenFader instance;
    CanvasGroup group;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        group = GetComponent<CanvasGroup>();
        value = 1.0f;
    }

    void Update()
    {
        group.alpha = value;
        value = Mathf.Clamp01(toBlack ? (value + Time.deltaTime) : (value - Time.deltaTime));
    }
    public void SetToBlack(bool v) => toBlack = v;
}

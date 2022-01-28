using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flickering from https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969
public class Candle : MonoBehaviour
{
    [SerializeField]
    private Light light;

    [SerializeField]
    public float minIntensity = 0.1f;
    [SerializeField]
    public float maxIntensity = 1f;
    [SerializeField]
    public int smoothing = 100;

    Queue<float> smoothQueue;
    float lastSum = 0;


    public bool on
    {
        get
        {
            return light.enabled;
        }
        set
        {
            light.enabled = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
    }

    // Update is called once per frame
    void Update()
    {
        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        light.intensity = lastSum / (float)smoothQueue.Count;
    }
}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class LightningManager : MonoBehaviour
{
    public static LightningManager Instance;
    Color onColour = Color.white;
    Color offColour = Color.black;
    public AudioSource audioSource;
    public LightningController[] lightningReceivers;

    private void Start()
    {
        InitialiseInstance();
        SetTargetColour(offColour);
        TurnOn();
    }

    void InitialiseInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    IEnumerator LightningCoroutine()
    {
        this.audioSource.Play();
        SetColour(onColour);
        yield return new WaitForSeconds(1f);
        SetColour(onColour);
        yield return new WaitForSeconds(0.5f);
        SetColour(onColour);
    }

    [ContextMenu("MakeLightningHappen")]
    public void MakeLightningHappen()
    {   
        StartCoroutine(LightningCoroutine());
    }

    [ContextMenu("Lightning On")]
    public void TurnOn()
    {
        foreach (LightningController lightningController in lightningReceivers)
        {
            lightningController.TurnOn();
        }
    }
    
    [ContextMenu("Lightning Off")]
    public void TurnOff()
    {
        foreach (LightningController lightningController in lightningReceivers)
        {
            lightningController.TurnOff();
        }
    }

    public void SetColour(Color newColour)
    {
        foreach (LightningController lightningController in lightningReceivers)
        {
            lightningController.SetColor(newColour);
        }
    }

    public void SetTargetColour(Color newColour)
    {
        foreach (LightningController lightningController in lightningReceivers)
        {
            lightningController.SetTargetColor(newColour);
        }
    }
}

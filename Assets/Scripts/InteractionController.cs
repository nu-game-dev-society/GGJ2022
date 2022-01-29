using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public LayerMask layers;
    public Transform cam;
    public float interactionReach;
    public IInteractable currentInteractable;
    [ColorUsage(true, true)]
    public Color EmissiveColor;
    private void Update()
    {
        if (
            Physics.Raycast(new Ray(cam.position, cam.forward), out RaycastHit hit, interactionReach, layers)
            &&
            hit.transform.TryGetComponent(out IInteractable interactable)
        )
        {
            if (interactable != currentInteractable)
            {
                SwitchInteractable(interactable);
            }
        }
        else if (currentInteractable != null)
            SwitchInteractable(null);

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
            currentInteractable.Select();

    }

    private void SwitchInteractable(IInteractable interactable)
    {
        if (currentInteractable != null)
            UnhighlightInteractable(currentInteractable);
        currentInteractable = interactable;
        if (currentInteractable != null)
            HighlightInteractable(currentInteractable);
        Debug.Log(currentInteractable);
    }

    private void HighlightInteractable(IInteractable interactable)
    {
        Material[] mats = interactable.GetMaterials();
        foreach (Material m in mats)
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", EmissiveColor);
        }
    }
    private void UnhighlightInteractable(IInteractable interactable)
    {
        Material[] mats = interactable.GetMaterials();
        foreach (Material m in mats)
        {
            m.DisableKeyword("_EMISSION");
        }
    }
}

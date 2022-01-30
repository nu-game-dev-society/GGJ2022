using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public LayerMask layers;
    public Transform cam;
    public float interactionReach;
    PlayerController player;
    public IInteractable currentInteractable;
    [ColorUsage(true, true)]
    public Color EmissiveColor;

    void Start()
    {
        this.player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (currentInteractable?.CanInteract(this.player) == false)
        {
            SwitchInteractable(null);
        }

        if (
            Physics.Raycast(new Ray(cam.position, cam.forward), out RaycastHit hit, interactionReach, layers)
            &&
            hit.transform.TryGetComponent(out IInteractable interactable)
        )
        {
            if (interactable != currentInteractable && interactable.CanInteract(this.player))
            {
                SwitchInteractable(interactable);
            }
        }
        else if (currentInteractable != null)
            SwitchInteractable(null);

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
            currentInteractable.Interact(this.player);
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
        IEnumerable<Material> mats = (interactable as MonoBehaviour).GetMaterials();
        foreach (Material m in mats)
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", EmissiveColor);
        }    
    }
    private void UnhighlightInteractable(IInteractable interactable)
    {
        IEnumerable<Material> mats = (interactable as MonoBehaviour).GetMaterials();
        foreach (Material m in mats)
        {
            m.DisableKeyword("_EMISSION");
        }
    }
}

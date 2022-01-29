using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerController interactor);
    bool CanInteract(PlayerController interactor);
}


public static class InteractableExtensions
{
    public static IEnumerable<Material> GetMaterials(this IInteractable interactable)
    {
        if (interactable is MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);
        }

        return Enumerable.Empty<Material>();
    }
}
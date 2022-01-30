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
    public static IEnumerable<Material> GetMaterials(this MonoBehaviour monoBehaviour)
        => monoBehaviour.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);
}
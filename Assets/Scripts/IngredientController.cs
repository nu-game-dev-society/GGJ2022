using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientController : MonoBehaviour, IInteractable
{
    public IEnumerable<Material> GetMaterials() => gameObject.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);

    public void Interact(PlayerController interactor)
    {
        gameObject.SetActive(false);
        interactor.Pickup(this);
    }

    public IngredientData Data;
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientController : MonoBehaviour, IInteractable
{
    public IEnumerable<Material> GetMaterials() => gameObject.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);

    public void Interact(PlayerController player)
    {
        gameObject.SetActive(false);
        //this.player.Pickup
    }

    
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientController : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController interactor)
    {
        interactor.Pickup(this);
    }
    public bool CanInteract(PlayerController interactor) => true;

    public IngredientData Data;
}

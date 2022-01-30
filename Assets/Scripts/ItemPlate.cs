using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlate : MonoBehaviour, IInteractable
{
    private IngredientController currentItem;

    public bool CanInteract(PlayerController interactor) => interactor.currentItem is IngredientController && (currentItem == null || currentItem.transform.parent != transform);

    public void Interact(PlayerController interactor)
    {
        IngredientController item = interactor.currentItem as IngredientController;
        if (item != null)
        {
            interactor.DropItem();
            SetCurrentItem(item);
        }
    }

    // Start is called before the first frame update
    public void SetCurrentItem(IngredientController item)
    {
        currentItem = item;
        item.transform.parent = transform;
        item.transform.position = transform.position;
        item.transform.rotation = item.initialRotation;
    }
}

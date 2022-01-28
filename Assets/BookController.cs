using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour, IInteractable
{
    public void Select()
    {
        gameObject.SetActive(false);
    }

    public void Use()
    {
        Debug.Log("USE ME");
    }
}

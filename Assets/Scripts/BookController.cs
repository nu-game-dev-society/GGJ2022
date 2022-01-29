using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookController : MonoBehaviour, IInteractable
{
    public IEnumerable<Material> GetMaterials() => gameObject.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);


    public void Interact()
    {
        gameObject.SetActive(false);
    }
}

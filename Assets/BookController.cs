using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour, IInteractable
{
    public Material[] GetMaterials()
    {
        List<Material> mats = new List<Material>();
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            mats.AddRange(r.materials);
        return mats.ToArray();
    }

    public void Select()
    {
        gameObject.SetActive(false);
    }
}

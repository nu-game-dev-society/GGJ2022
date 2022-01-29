using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BookController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TextMeshPro cover;
    [SerializeField]
    private TextMeshPro inside;
    [SerializeField]
    private TextMeshPro bind;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    private Material[] materials;

    public void Interact(PlayerController interactor)
    {
        gameObject.SetActive(false);
        interactor.Pickup(this);
    }

    void Start()
    {
        cover.text = BookData.RandomTitle() + "\n\n\n\n\n\n" + BookData.RandomSubTitle();
        meshRenderer.material = materials[Random.Range(0, materials.Length - 1)];
    }
}

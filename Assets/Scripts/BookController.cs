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

    public IEnumerable<Material> GetMaterials() => gameObject.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);

    public void Interact(PlayerController player)
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        cover.text = BookData.RandomTitle() + "\n\n\n\n\n\n" + BookData.RandomSubTitle();
        meshRenderer.material = materials[Random.Range(0, materials.Length - 1)];
    }
}

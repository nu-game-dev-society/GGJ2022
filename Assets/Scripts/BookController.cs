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

    private string title;
    private string subtitle;
    private Animator animator;

    public void Interact(PlayerController interactor)
    {
        interactor.Pickup(this);
    }

    internal void SetContents(string contents)
    {
        inside.text = contents;
    }

    public bool CanInteract(PlayerController interactor) => true;

    void Start()
    {
        animator = GetComponent<Animator>();

        title = BookData.RandomTitle();
        subtitle = BookData.RandomSubTitle();
        cover.text = title + "\n\n\n\n\n\n" + subtitle;
        bind.text = title.Replace("\n", " ") + "\n" + subtitle;
        meshRenderer.material = materials[Random.Range(0, materials.Length - 1)];
        gameObject.name = GetName();
    }

    public IEnumerator Open(bool state = true)
    {
        animator.SetBool("Open", state);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public string GetName()
    {
        return title.Replace("\n", " ").Trim() + ": " + subtitle;
    }
}

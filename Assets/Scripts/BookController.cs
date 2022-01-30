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

    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer basicRenderer;

    [SerializeField]
    private Material[] materials;

    private string title;
    private string subtitle;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip openClip;
    public AudioClip closeClip;

    public Collider collider;

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
        meshRenderer.sharedMaterial = materials[Random.Range(0, materials.Length - 1)]; 
        basicRenderer.sharedMaterial = meshRenderer.sharedMaterial;
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

    [ExecuteAlways]
    public void OpenClose()
    {
        audioSource.PlayOneShot(animator.GetBool("Open") ? openClip : closeClip);
    }
}

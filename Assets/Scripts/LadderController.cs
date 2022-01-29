using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LadderController : MonoBehaviour, IInteractable
{
    public bool inUse;
    public IEnumerable<Material> GetMaterials() => gameObject.GetComponentsInChildren<Renderer>().SelectMany(renderer => renderer.materials);

    public float speed;
    public Transform playerParent;

    public Vector3 ladderStartPos;
    public Vector3 ladderEndPos;
    private float ladderT;

    public void Interact(PlayerController player)
    {
        inUse = !inUse;
        enabled = inUse;
        player.SetInUse(!inUse);
        player.transform.parent = inUse ? playerParent : null;

        if (inUse)
            player.transform.localPosition = Vector3.zero;

    }
    public bool CanInteract(PlayerController interactor) => true;

    void Update()
    {
        if (inUse)//should always be true tbh
        {
            ladderT += Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed / Vector3.Distance(ladderStartPos, ladderEndPos);
            ladderT = Mathf.Clamp01(ladderT);

            transform.position = Vector3.Lerp(ladderStartPos, ladderEndPos, ladderT);
        }
    }
}

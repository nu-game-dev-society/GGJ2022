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
    public Transform topOfLadder;

    public Vector3 ladderStartPos;
    public Vector3 ladderEndPos;
    public Vector3 playerTargetPosition = Vector3.zero;
    private float ladderT;
    public bool isAtBottom;
    private PlayerController player;

    public Vector3 lookRotation;
    public void Interact(PlayerController player)
    {
        if (inUse && !isAtBottom)
            return;

        inUse = !inUse;
        enabled = inUse;

        if (!inUse)
        {
            playerParent.localPosition = playerTargetPosition; ;
            player.transform.position = transform.TransformPoint(playerTargetPosition);
        }

        player.SetInUse(!inUse);

        lookRotation = player.cam.localEulerAngles;

        player.transform.parent = inUse ? playerParent : null;

        this.player = inUse ? player : null;
        if (inUse)
        {
            player.transform.localPosition = Vector3.zero;
            playerTargetPosition = Vector3.zero;
        }
    }
    void Update()
    {
        if (inUse)//should always be true tbh
        {
            MoveLadder();
            RotatePlayerCamera();
            PlayerLadderClimb();
        }
    }

    private void PlayerLadderClimb()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isAtBottom)
            {
                playerTargetPosition = topOfLadder.localPosition;
                isAtBottom = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isAtBottom)
            {
                playerTargetPosition = Vector3.zero;
                isAtBottom = true;
            }
        }
        playerParent.localPosition = Vector3.Lerp(playerParent.localPosition, playerTargetPosition, Time.deltaTime);
    }

    private void RotatePlayerCamera()
    {
        if (player)
        {
            lookRotation.y += Input.GetAxisRaw("Mouse X") * Time.deltaTime;
            lookRotation.x -= Input.GetAxis("Mouse Y") * Time.deltaTime;
            lookRotation.x = Mathf.Clamp(lookRotation.x, -60, 60);
            player.cam.localEulerAngles = lookRotation;
        }
    }

    private void MoveLadder()
    {
        if (!isAtBottom)
            return;
        ladderT += Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed / Vector3.Distance(ladderStartPos, ladderEndPos);
        ladderT = Mathf.Clamp01(ladderT);

        transform.position = Vector3.Lerp(ladderStartPos, ladderEndPos, ladderT);
    }
}

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


    public AudioSource audiosource;
    public AudioClip ladderLoop;
        
    public Vector3 lookRotation;

    public float climbSpeed = 0.5f;

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
            player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y + lookRotation.y, 0);
        }

        player.SetInUse(!inUse);

        lookRotation = player.cam.localEulerAngles;

        player.transform.parent = inUse ? playerParent : null;

        this.player = inUse ? player : null;
        if (inUse)
        {
            player.transform.localPosition = Vector3.zero;
            playerTargetPosition = Vector3.zero;
            firstFrameEnabled = true;
        }
    }
    void Update()
    {
        if (inUse)//should always be true tbh
        {
            MoveLadder();
            RotatePlayerCamera();
            PlayerLadderClimb();
            firstFrameEnabled = false;

            if (!audiosource.isPlaying)
            {
                audiosource.clip = ladderLoop;
                audiosource.loop = true;
                audiosource.volume = 0;
                audiosource.Play();
            }
        }
        else
        {
            if (audiosource.isPlaying)
            {
                audiosource.Stop();
            }
        }
    }
    bool firstFrameEnabled = false;
    private void PlayerLadderClimb()
    {
        if (isAtBottom && (Input.GetKey(KeyCode.S) || (firstFrameEnabled == false && Input.GetKeyDown(KeyCode.E))))
        {
            Interact(player);
        }

        if (Input.GetKey(KeyCode.W))
        {
            playerTargetPosition.y += climbSpeed * Time.deltaTime;
            isAtBottom = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerTargetPosition.y -= climbSpeed * Time.deltaTime;
            isAtBottom = playerParent.localPosition.y <= 0f;

        }

        playerTargetPosition.y = Mathf.Clamp(playerTargetPosition.y, 0, topOfLadder.localPosition.y);

        playerParent.localPosition = playerTargetPosition;
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
        
        ladderT += Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed / Vector3.Distance(ladderStartPos, ladderEndPos);
        ladderT = Mathf.Clamp01(ladderT);

        transform.position = Vector3.Lerp(ladderStartPos, ladderEndPos, ladderT);

        audiosource.volume = ladderT != 0 && ladderT != 1 ? Mathf.Abs(Input.GetAxisRaw("Horizontal")) * 0.8f : 0;
    }

    public bool CanInteract(PlayerController interactor)
    {
        return !inUse;
    }
}

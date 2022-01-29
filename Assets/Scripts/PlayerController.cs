using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    public Transform cam;
    public float speed;
    public IInteractable currentItem;
    public Transform heldItemRoot;

    public AudioSource audioSource;
    public AudioClip[] stepClips;
    public float timeBetweenSteps = 0.1f;
    private int lastStepClip;
    private float lastStepTime;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    float mouseY;
    void LateUpdate()
    {
        Vector3 dir = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal")).normalized;
        cc.Move(speed * Time.deltaTime * dir);

        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * Time.deltaTime, 0));
        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -60, 60);
        cam.localEulerAngles = new Vector3(-mouseY, 0, 0);

        if (dir.magnitude > 0 && Time.time > lastStepTime + timeBetweenSteps)
        {
            PlayStep();
            lastStepTime = Time.time;
        }
    }

    public void SetInUse(bool enabled)
    {
        this.enabled = enabled;
        cc.enabled = enabled;
        if (enabled)
        {
            cam.localEulerAngles = Vector3.zero;
            mouseY = 0;
        }
    }

    public void PlayStep()
    {
        int i = UnityEngine.Random.Range(0, stepClips.Length);
        if (i == lastStepClip)
            i++;

        if (i > stepClips.Length)
            i++;

        audioSource.PlayOneShot(stepClips[i]);

        lastStepClip = i;
    }

    public void Pickup(IInteractable interactable)
    {
        if (this.currentItem == null)
        {
            this.currentItem = interactable;
            if (this.currentItem is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.gameObject.transform.parent = this.heldItemRoot;
                monoBehaviour.gameObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}

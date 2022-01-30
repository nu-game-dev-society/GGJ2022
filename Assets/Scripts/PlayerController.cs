using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    public Transform camTransform;
    public float speed;
    public IInteractable currentItem;
    public Transform heldItemRoot;

    public AudioSource audioSource;
    public AudioClip[] stepClips;
    public float timeBetweenSteps = 0.1f;
    private int lastStepClip;
    private float lastStepTime;
    float startFoV;
    Camera cam;

    

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        startFoV = Camera.main.fieldOfView;
        cam = Camera.main;
    }

    internal void DropItem()
    {
        if (this.currentItem is MonoBehaviour monoBehaviour)
        {
            SetLayerRecursively(monoBehaviour.gameObject, LayerMask.NameToLayer("Interactable"));
        }

        this.currentItem = null;
    }

    float mouseY;
    void Update()
    {
        Vector3 dir = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal")).normalized;
        Vector3 gravity = cc.isGrounded ? Vector3.zero : 9.8f * Time.deltaTime * Vector3.down;
        cc.Move((speed * Time.deltaTime * dir) + gravity);

        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0));
        mouseY += Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, -60, 60);
        camTransform.localEulerAngles = new Vector3(-mouseY, 0, 0);

        if (dir.magnitude > 0 && Time.time > lastStepTime + timeBetweenSteps)
        {
            PlayStep();
            lastStepTime = Time.time;
        }


        float targetFoV = Input.GetMouseButton(1)
            ? startFoV * 0.5f
            : startFoV;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFoV, Time.deltaTime * 3f);
    }

    public void SetInUse(bool enabled)
    {
        this.enabled = enabled;
        cc.enabled = enabled;
        if (enabled)
        {
            camTransform.localEulerAngles = Vector3.zero;
            mouseY = 0;
        }
    }

    public void PlayStep()
    {
        int i = UnityEngine.Random.Range(0, stepClips.Length);
        if (i == lastStepClip)
            i++;

        if (i >= stepClips.Length)
            i = 0;

        audioSource.PlayOneShot(stepClips[i]);

        lastStepClip = i;
    }

    public void Pickup(IInteractable interactable, AudioClip sound)
    {
        if (this.currentItem == null)
        {
            this.currentItem = interactable;

            if (this.currentItem is MonoBehaviour monoBehaviour)
            {
                audioSource.PlayOneShot(sound);

                monoBehaviour.gameObject.transform.parent = this.heldItemRoot;
                monoBehaviour.gameObject.transform.localPosition = Vector3.zero;

                SetLayerRecursively(monoBehaviour.gameObject, LayerMask.NameToLayer("PickedUp"));
            }
        }
    }

    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}

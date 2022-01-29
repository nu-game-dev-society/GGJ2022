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
    }

    public void SetInUse(bool enabled)
    {
        this.enabled = enabled;
    }
}

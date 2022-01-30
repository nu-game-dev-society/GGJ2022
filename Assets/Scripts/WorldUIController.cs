using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIController : MonoBehaviour
{
    internal IButtonEvents currentButton;
    public LayerMask layers;
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        if (
    Physics.Raycast(r, out RaycastHit hit, 100.0f, layers)
    &&
    hit.transform.TryGetComponent(out IButtonEvents interactable)
)
        {
            if (interactable != currentButton)
            {
                SwitchInteractable(interactable);
            }
        }
        else if (currentButton != null)
            SwitchInteractable(null);

        if (currentButton != null && Input.GetMouseButtonDown(0))
            currentButton.OnClicked();

    }
    private void SwitchInteractable(IButtonEvents interactable)
    {
        if (currentButton != null)
            currentButton.OnHoverExit();
        currentButton = interactable;
        if (currentButton != null)
            currentButton.OnHoverEnter();
    }
}

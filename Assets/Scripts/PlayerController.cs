using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    public PlayerControls playerControls;
    public LayerMask groundLayer;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Fire.performed += LeftClicked;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Fire.performed -= LeftClicked;
    }


    private void LeftClicked(InputAction.CallbackContext context)
    {
        MouseOperations.ClickedObject2D().click();
    }
}

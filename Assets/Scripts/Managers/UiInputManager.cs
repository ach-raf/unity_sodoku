using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInputManager : MonoBehaviour
{
    public GameObject canvas;

    private PointerEventData clickData;
    private List<RaycastResult> clickResults;
    private GraphicRaycaster uiRaycaster;
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
        //GetUiElementsClicked();
    }

    private void Start()
    {
        uiRaycaster = canvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
    }

    public void GetUiElementsClicked()
    {
        clickData.position = Mouse.current.position.ReadValue();
        clickResults.Clear(); uiRaycaster.Raycast(clickData, clickResults);
        foreach (RaycastResult result in clickResults)
        {
            GameObject ui_element = result.gameObject;
            Debug.Log($"UI Manager: {ui_element.name}");
            if (ui_element.TryGetComponent<IClickable>(out IClickable clicked_object))
            {

                clicked_object?.click();
            }
        }
    }
}

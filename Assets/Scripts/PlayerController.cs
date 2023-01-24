using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
	public PlayerControls playerControls;
	public LayerMask groundLayer;
	public RaycastHit mouseHit;
	public Vector2 cursorPosition = new Vector2();

	private void Awake()
	{
		playerControls = new PlayerControls();
	}
	private void OnEnable()
	{
		playerControls.Enable();
		playerControls.Player.Fire.performed += LeftClicked;
		playerControls.Player.Position.performed += CursorMoved;
	}

	private void OnDisable()
	{
		playerControls.Disable();
		playerControls.Player.Fire.performed -= LeftClicked;
		playerControls.Player.Position.performed -= CursorMoved;
	}


	private void LeftClicked(InputAction.CallbackContext context)
	{
		/*if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/
		if (ClickedObject2D() != null)
		{
			ClickedObject2D().click();
		}

	}

	public void CursorMoved(InputAction.CallbackContext context)
	{
		cursorPosition = context.ReadValue<Vector2>();
	}

	public IClickable ClickedObject2D()
	{
		Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
		if (hit.collider)
		{
			IClickable clicked_object = hit.transform.GetComponentInChildren<IClickable>();
			return clicked_object;
		}
		else
		{
			Debug.Log("No object clicked");
			return null;
		}
	}
}

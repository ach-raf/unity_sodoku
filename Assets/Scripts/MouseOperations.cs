using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseOperations
{

	private static PlayerController playerController;
	public static RaycastHit mouseHit;





	public static IClickable GetClickedObject(LayerMask ground_mask)
	{
		try
		{
			RaycastHit hit = GroundMousePosition(ground_mask);
			if (hit.collider != null)
			{
				IClickable clicked_object = hit.collider.GetComponent<IClickable>();
				if (clicked_object != null)
				{
					return clicked_object;
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}

		}
		catch (System.NullReferenceException)
		{
			return null;
		}
	}

	public static RaycastHit GroundMousePosition(LayerMask ground_mask)
	{
		Vector3 mousePosFar = Mouse.current.position.ReadValue();
		mousePosFar.z = Camera.main.farClipPlane;

		Vector3 mousePosNear = Mouse.current.position.ReadValue();
		mousePosNear.z = Camera.main.nearClipPlane;

		Vector3 WorldPosFar = Camera.main.ScreenToWorldPoint(mousePosFar);
		Vector3 WorldPosNear = Camera.main.ScreenToWorldPoint(mousePosNear);

		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (Physics.Raycast(ray, out RaycastHit ground_ray_hit, float.MaxValue, ground_mask))
		{

		}
		return ground_ray_hit;
	}
	public static RaycastHit CastRay()
	{
		Vector3 mousePosFar = Mouse.current.position.ReadValue();
		mousePosFar.z = Camera.main.farClipPlane;

		Vector3 mousePosNear = Mouse.current.position.ReadValue();
		mousePosNear.z = Camera.main.nearClipPlane;

		Vector3 WorldPosFar = Camera.main.ScreenToWorldPoint(mousePosFar);
		Vector3 WorldPosNear = Camera.main.ScreenToWorldPoint(mousePosNear);

		/*RaycastHit hit;
        Physics.Raycast(WorldPosNear, WorldPosFar - WorldPosNear, out hit);
        return hit;*/

		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
		{

		}
		return hit;
	}
	public static IClickable ClickedObject()
	{
		RaycastHit hit = CastRay();
		if (hit.collider != null)
		{
			//Debug.Log(hit.collider.gameObject.name);
			IClickable clicked_object = hit.transform.GetComponent<IClickable>();
			if (clicked_object != null)
			{
				return clicked_object;
			}
			else
			{
				return null;
			}
		}
		else
		{
			return null;
		}
	}

	public static IClickable ClickedObject2D()
	{
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
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

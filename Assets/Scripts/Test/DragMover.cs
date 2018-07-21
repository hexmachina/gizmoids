using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragMover : MonoBehaviour
{

	public void OnPointerEvent(Vector3 position)
	{
		transform.position = new Vector3(position.x, position.y, transform.position.z);
	}

	public void OnPointerUp(Vector3 position)
	{

	}

	public void ColorToggle(bool isOn)
	{
		var img = GetComponent<UnityEngine.UI.Image>();
		if (!img)
		{
			return;
		}
		if (isOn)
		{
			img.color = Color.green;
		}
		else
		{
			img.color = Color.white;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Background : MonoBehaviour
{
	public float boostSpeed = 0.05f;

	public int worldId;
	public List<BGParrallax> backgrounds;

	public float direction;

	public Transform targetDirection;

	private float speedOffset = 0;

	void Awake()
	{
		backgrounds = GetBGParrallax();
	}

	// Update is called once per frame
	void Update()
	{
		MoveBackgrounds();
	}

	public void MoveBackgrounds()
	{
		if (targetDirection)
		{
			direction = targetDirection.transform.eulerAngles.z;
		}

		for (int i = 0; i < backgrounds.Count; i++)
		{
			backgrounds[i].Move(backgrounds[i].scrollSpeed + speedOffset, direction);
		}
	}

	public void OnThrust(bool active)
	{
		if (active)
		{
			speedOffset = boostSpeed;
		}
		else
		{
			speedOffset = 0;
		}
	}

	public void AdjustScrollSpeeds(float speed)
	{
		for (int i = 0; i < backgrounds.Count; i++)
		{
			backgrounds[i].scrollSpeed += speed;
		}
	}

	public void ResetScrollSpeeds()
	{
		foreach (var item in backgrounds)
		{
			item.resetSpeed();
		}

	}

	public List<BGParrallax> GetBGParrallax()
	{
		List<BGParrallax> list = new List<BGParrallax>();
		foreach (var item in GetComponentsInChildren<BGParrallax>())
		{
			list.Add(item);
		}
		return list;
	}
}

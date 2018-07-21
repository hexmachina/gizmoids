using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectibleInteractive : CollectibleView
{
	private CollectibleButtonIcon buttonIcon;

	private bool firstMobilize = true;

	public override void Mobilize(int dir)
	{
		if (firstMobilize)
		{
			mover.onTargetReached.AddListener(TargetReached);
			firstMobilize = false;
		}
		if (dropped && collectibleData.moverData.dropInBounds)
		{
			var radius = Vector3.Distance(Vector3.zero, transform.position);
			float endPoint = -1;
			if (radius > collectibleData.moverData.maxDropRadius)
			{
				endPoint = collectibleData.moverData.maxDropRadius;
			}
			else if (radius < collectibleData.moverData.minDropRadius)
			{
				endPoint = collectibleData.moverData.minDropRadius;
			}
			if (endPoint == collectibleData.moverData.maxDropRadius || endPoint == collectibleData.moverData.minDropRadius)
			{
				var degrees = Mathfx.GetAngle(Vector3.zero, transform.position);
				mover.SetAngle(degrees);
				mover.SetEndPoint(endPoint);
				mover.StartMovement(MoveType.Path, collectibleData.moverData.speed, mover.endPoint);
			}
			else
			{
				TargetReached();
			}
		}
		else
		{
			mover.Mobilize(collectibleData.moverData.speed);
			if (!dropped)
			{
				mover.GoToStartPoint();
			}
			if (collectibleData.moverData.orientRotation)
			{
				var degrees = Mathfx.GetAngle(Vector3.zero, transform.position);
				transform.eulerAngles = new Vector3(0, 0, degrees - 90);
			}
			mover.moving = true;
		}
	}

	protected override void TargetReached()
	{
		Debug.Log("Target Reached");
		if (phasesOut)
		{
			Phase(collectibleData.phaseCount, Hide, collectibleData.phaseDelay);
		}
	}

	public void BuildButton(Transform parent)
	{
		if (buttonIcon)
		{
			buttonIcon.gameObject.SetActive(true);
			StartCoroutine(ButtonFollower());
		}
		else
		{
			if (collectibleData.buttonIcon)
			{
				buttonIcon = Instantiate(collectibleData.buttonIcon, parent);
				buttonIcon.transform.localScale = Vector3.one;
				buttonIcon.button.onClick.AddListener(PickUp);
				if (buttonIcon.icon && collectibleData.icon)
				{
					buttonIcon.icon.sprite = collectibleData.icon;
				}
				StartCoroutine(ButtonFollower());
			}
		}
	}

	IEnumerator ButtonFollower()
	{
		while (buttonIcon && buttonIcon.gameObject.activeSelf)
		{
			buttonIcon.transform.position = Camera.main.WorldToScreenPoint(transform.position);
			yield return null;
		}
	}

	public void PickUp()
	{
		Collect();
		if (collectibleData)
		{
			AudioPlayer.Instance.PlaySoundClip(collectibleData.pickupClip);
		}
		if (FXManager.instance)
		{
			FXManager.instance.AddPointer(transform.position);
		}

		Hide();
	}

	public void Hide()
	{
		if (buttonIcon)
		{
			buttonIcon.gameObject.SetActive(false);
		}
		gameObject.SetActive(false);
	}
}

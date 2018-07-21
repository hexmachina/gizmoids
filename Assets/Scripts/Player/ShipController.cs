using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ShipController : MonoBehaviour
{
	[System.Serializable]
	public class FloatEvent : UnityEvent<float> { }

	public float maxRotationSpeed = 1; //In degrees

	public float snapDuration;

	public FloatEvent onEndRotate = new FloatEvent();

	private float pointerDownOffset;

	private bool dragging;

	Sequence snapSequence;

	public void OnPointerDown(Vector3 position)
	{
		pointerDownOffset = GetRotationOffset(position);
		dragging = true;
		StartCoroutine(RotateByInterface());
	}

	public void OnPointerUp(Vector3 position)
	{
		dragging = false;
		SnapMotion();
	}

	public void SnapMotion()
	{
		var snap = SnapRotation();
		onEndRotate.Invoke(snap);
		snapSequence = DOTween.Sequence();
		snapSequence.Append(transform.DORotate(new Vector3(0, 0, snap), snapDuration).SetEase(DG.Tweening.Ease.OutElastic));

	}

	public void ForceRotate(float value, float duration, bool snapping)
	{
		if (duration == 0)
		{
			transform.eulerAngles = new Vector3(0, 0, value);
			if (snapping)
			{
				SnapMotion();
			}
		}
		else
		{
			snapSequence = DOTween.Sequence();
			snapSequence.Append(transform.DORotate(new Vector3(0, 0, value), duration));
			if (snapping)
			{
				snapSequence.OnComplete(SnapMotion);
			}
		}
	}

	private IEnumerator RotateByInterface()
	{
		while (dragging)
		{
			RotateStepTowardsTarget(GetTargetAngleZ(GizLib.Helper.GetWorldPoint()) - pointerDownOffset);
			yield return null;
		}
	}

	public float SnapRotation()
	{
		float newRotation = 0f;
		int divisible = (int)(transform.eulerAngles.z / 45);

		float leftOver = Mathf.Abs(transform.eulerAngles.z % 45);
		if (leftOver > 22.5)
		{
			divisible++;
		}
		if (transform.eulerAngles.z > 0)
		{
			newRotation = 45 * divisible;
		}
		else
		{
			newRotation = -45 * divisible;
		}
		if (newRotation == 360)
		{
			newRotation = 0;
		}

		return newRotation;
	}

	public float GetTargetAngleZ(Vector3 targetPosition)
	{
		return Quaternion.LookRotation(transform.position - targetPosition, Vector3.forward).eulerAngles.z;
	}

	public float GetRotationOffset(Vector3 targetPosition)
	{
		var lastAngleZ = transform.eulerAngles.z;
		float rawOffset = GetTargetAngleZ(targetPosition) - lastAngleZ;
		return Mathfx.Modulo(rawOffset, 360);
	}

	public void RotateStepTowardsTarget(float rawTargetZAngle)
	{
		float clampedTargetZAngle = Mathfx.Modulo(rawTargetZAngle, 360);
		float maxShipRotationPerFrame = maxRotationSpeed;

		//If difference between target and current z angle is not less than degrees per frame, begin moving
		if (!(transform.eulerAngles.z - clampedTargetZAngle < maxShipRotationPerFrame && transform.rotation.eulerAngles.z - clampedTargetZAngle > -maxShipRotationPerFrame))
		{
			//If angle from current to target clockwise is less than or equal to 180, turn right.
			if (Mathfx.Modulo(clampedTargetZAngle - transform.eulerAngles.z, 360) > 180)
			{
				transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - maxShipRotationPerFrame);
			}
			else
			{
				//Otherwise, turn left
				transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + maxShipRotationPerFrame);
			}
		}
		else
		{
			//Since target z angle is less than max degrees per frame away, simply set z angle to target
			transform.eulerAngles = new Vector3(0, 0, clampedTargetZAngle);
		}
	}
}

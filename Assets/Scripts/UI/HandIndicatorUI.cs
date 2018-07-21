using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class HandIndicatorUI : MonoBehaviour
{

	public UnityEvent onPressComplete = new UnityEvent();

	public UnityEvent onReleaseComplete = new UnityEvent();

	public Animator animator;

	public Sequence sequence;

	public float loopDelay = 2;

	private Vector2 origin;

	private Transform target;

	private float radius;

	private bool setup;

	private void Awake()
	{
		if (!animator)
		{
			animator = GetComponent<Animator>();
		}
	}

	private void OnEnable()
	{
		if (setup)
		{
			StartMotion();
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	public void PressComplete()
	{
		Debug.Log("PressComplete");
		onPressComplete.Invoke();
	}

	public void ReleaseComplete()
	{
		Debug.Log("ReleaseComplete");
		onReleaseComplete.Invoke();
	}

	public void Press()
	{
		if (animator)
		{
			animator.SetTrigger("Press");
		}
	}

	public void Release()
	{
		if (animator)
		{
			animator.SetTrigger("Release");
		}
	}

	public void SetupMotion(Vector2 startPos, Transform trans, float radiusOffset, bool loop = false, float delay = 2f)
	{
		setup = true;
		onPressComplete.RemoveAllListeners();
		onReleaseComplete.RemoveAllListeners();

		origin = startPos;

		target = trans;

		radius = radiusOffset;

		onPressComplete.AddListener(MoveToTarget);
		if (loop)
		{
			loopDelay = delay;
			onReleaseComplete.AddListener(LoopMotion);
		}

		StartMotion();
	}

	private IEnumerator Wait(float duration, System.Action callback)
	{
		yield return new WaitForSeconds(duration);
		if (callback != null)
		{
			callback.Invoke();
		}
	}

	public void LoopMotion()
	{
		StartCoroutine(Wait(loopDelay, StartMotion));
	}

	public void StartMotion()
	{
		var rt = transform as RectTransform;
		rt.anchoredPosition = origin;
		Press();
	}

	private void MoveToTarget()
	{
		sequence = DOTween.Sequence();
		var rt = transform as RectTransform;
		var circ = Mathfx.GetPointOnCircle(radius, target.eulerAngles.z + 90, Vector3.zero);
		var point = Camera.main.WorldToScreenPoint(circ);
		sequence.Append(transform.DOMove(point, 0.7f).SetEase(DG.Tweening.Ease.InOutQuad));
		sequence.OnComplete(Release);

	}

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GizLib;

public class CollectibleTracker : CollectibleView
{
	public UnityEngine.UI.Button buttonPrefab;

	public float timeLimit;

	public UnityEngine.UI.Button button;

	public AudioClip pickupClip;
	public List<SpriteRenderer> renderers;

	// Use this for initialization
	void Start()
	{
		var sp = GetComponent<SpriteRenderer>();
		if (sp != null)
		{
			renderers.Add(sp);
		}
		var sps = GetComponentsInChildren<SpriteRenderer>();
		foreach (var render in sps)
		{
			if (render != null)
			{
				renderers.Add(render);
			}
		}
		if (!mover)
		{
			mover = GetComponent<Mover>();
		}
		//Mobilize();
		SetupCollectible();
	}

	public void BuildButton(Transform parent)
	{
		if (button)
		{
			button.gameObject.SetActive(true);
			StartCoroutine(ButtonFollower());
		}
		else
		{
			button = Instantiate(buttonPrefab, parent);
			button.transform.localScale = Vector3.one;
			button.onClick.AddListener(HandleCollect);
			StartCoroutine(ButtonFollower());
		}
	}

	IEnumerator ButtonFollower()
	{
		while (button && button.gameObject.activeSelf)
		{
			button.transform.position = Camera.main.WorldToScreenPoint(transform.position);
			yield return null;
		}
	}

	public void HandleCollect()
	{
		Collect();
		AudioPlayer.Instance.PlaySoundClip(pickupClip);
		if (FXManager.instance)
		{
			FXManager.instance.AddPointer(transform.position);
		}

		Hide();
	}

	public void SetupCollectible()
	{
		if (!collectibleData)
		{
			return;
		}
		switch (collectibleData.collectibleType)
		{
			case CollectibleType.BigScrap:
				mover.onTargetReached.AddListener(TargetReached);
				if (!dropped)
				{
					mover.GoToStartPoint();
				}
				transform.eulerAngles = new Vector3(0, 0, mover.degree - 90);
				mover.moving = true;
				break;
			case CollectibleType.Blueprint:
				var degree = Mathfx.GetAngle(Vector3.zero, transform.position);
				var radius = Vector3.Distance(transform.position, Vector3.zero);
				print("blueprint radius: " + radius + " angle: " + degree);
				if (radius > 4.5f)
				{
					mover.SetAngle(degree);
					mover.SetEndPoint(3.5f);
					mover.StartMovement(MoveType.Path, 3, mover.endPoint);
				}

				break;
			case CollectibleType.Xeno:

				if (phasesOut)
				{
					StartTimeLimit();
				}

				var xDegree = Mathfx.GetAngle(Vector3.zero, transform.position);
				var distance = Vector3.Distance(transform.position, Vector3.zero);

				if (distance > 4.5)
				{
					mover.SetAngle(xDegree);
					mover.SetEndPoint(4f);
					mover.StartMovement(MoveType.Path, 3, mover.endPoint);
				}
				break;
		}
	}

	protected override void TargetReached()
	{
		switch (collectibleData.collectibleType)
		{
			case CollectibleType.BigScrap:
				if (phasesOut)
				{
					StartCoroutine(PhasingOut(phaseCount));
				}
				break;
		}

	}

	public void StartTimeLimit()
	{
		StartCoroutine(TimeLimit(timeLimit));
	}

	IEnumerator TimeLimit(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		StartCoroutine(PhasingOut(phaseCount));
	}

	IEnumerator PhasingOut(int phases)
	{
		int phase = 0;
		float rate = 2f;

		while (phase < phases)
		{
			if (phase % 2 == 0)
			{
				foreach (var sp in renderers)
				{
					if (sp.material.color.a > 0)
					{

						sp.material.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(sp.material.color.a, 0, rate * Time.deltaTime));

					}
					else
					{
						phase++;
					}
				}
			}
			else
			{
				foreach (var sp in renderers)
				{
					if (sp.material.color.a < 1)
					{
						sp.material.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(sp.material.color.a, 1, rate * Time.deltaTime));

					}
					else
					{
						phase++;
					}
				}
			}
			yield return null;
		}
		if (button)
		{
			button.gameObject.SetActive(false);
		}
		gameObject.SetActive(false);
	}

	public void Hide()
	{
		if (button)
		{
			button.gameObject.SetActive(false);
		}
		gameObject.SetActive(false);
	}

	//void ActionCheckOut()
	//{
	//	FXManager.instance.AddPointer(transform.position);
	//	LevelManager.Instance.itemsCollected++;
	//	switch (collectibleData.collectibleType)
	//	{
	//		case CollectibleType.BigScrap:
	//			PlayerResources.Instance.IncrementScrap(collectibleData.value);
	//			break;
	//		case CollectibleType.Blueprint:
	//			LevelManager.Instance.blueprintsCollected++;
	//			print("Gizmoid " + collectibleData.value);
	//			if (collectibleData.value == -1)
	//			{
	//				break;
	//			}

	//			if (!LevelManager.Instance.gameProfile.gizmoids.Contains(collectibleData.value))
	//			{
	//				var ip = Instantiate(InterfaceController.Instance.infoPanel) as InfoPanel;
	//				ip.transform.parent = InterfaceController.Instance.panelPopup.transform;
	//				ip.transform.localPosition = Vector3.zero;
	//				ip.transform.localScale = Vector3.one;
	//				//ip.actDriven = true;
	//				ip.AddGizmoidInfo(collectibleData.value, "New Gizmoid: ");
	//				LevelManager.Instance.gameProfile.gizmoids.Add(collectibleData.value);
	//			}
	//			break;
	//		case CollectibleType.Xeno:
	//			PlayerResources.Instance.AddXenos(collectibleData.value);
	//			LevelManager.Instance.xenoCollected += collectibleData.value;
	//			break;
	//		case CollectibleType.Overclocker:
	//			PlayerResources.Instance.AddOverclockToken();
	//			break;
	//	}

	//	AudioPlayer.Instance.PlaySoundClip(pickupClip, transform);
	//	Destroy(gameObject);
	//	//print("collected");
	//}

}

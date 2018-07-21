using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PopState
{
	Popped = 0,
	Popping = 1,
	Unpopping = 2,
	Normal = 3
}

public class Repel : MonoBehaviour
{
	//public PopState popState = PopState.n
	public bool activated;
	public bool popped;

	public float initialRecoverTime;
	public float recoverTime;
	public float recoverIncrement;

	public float repelSpeed;
	public int repelSpan;
	//public float repelRadius;

	//public int popCounter;
	//public int popTrigger;

	private bool popping;
	private bool unpopping;

	//public CheckArena checkArena;
	public GizmoidView tracker;

	// Use this for initialization
	void Start()
	{
		//checkArena = GetComponent<CheckArena>();
		recoverTime = initialRecoverTime;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		HandleRepel();
	}

	void SetActivation(bool f)
	{
		activated = f;
	}



	void HandleRepel()
	{
		if (popped)
		{

			if (!unpopping)
			{
				StartCoroutine(Unpopping(recoverTime));
				unpopping = true;
			}


		}
		else
		{
			//if (checkArena.CheckAisle())
			//{
			//	if (!popping)
			//	{
			//		StartCoroutine(Popping());
			//		popping = true;
			//	}
			//}
		}


	}

	IEnumerator Popping()
	{
		AudioPlayer.Instance.PlaySoundClip(tracker.assets.clipUtility1, transform);
		//tracker.animationManager.anim.SetTrigger("Active");           
		tracker.unitGraphics.anim.Play("Balloon_IdleToReady_01", 0, 0f);

		yield return new WaitForSeconds(1.53f);

		//if (checkArena.CheckAisle())
		//{
		//	//tracker.animationManager.anim.SetTrigger("Inflate");
		//	tracker.animationManager.anim.Play("Active", 0, 0f);

		//	popped = true;
		//}
		yield return new WaitForSeconds(0.75f);
		AudioPlayer.Instance.PlaySoundClip(tracker.assets.clipUtility2, transform);
		//if (checkArena.CheckAisle())
		//{

		//	PushAway();
		//}
		popping = false;
		yield return null;
	}

	void PushAway()
	{
		//foreach (var item in checkArena.GetListeningRows())
		//{
		//	foreach (var repeller in item.FindOccupiers("Enemy Point"))
		//	{
		//		if (repeller != null)
		//		{
		//			var enemy = repeller.GetComponentInParent<EnemyView>();
		//			int repelDist = enemy.arenaPlacement.row + repelSpan;
		//			if (repelDist > 7)
		//			{
		//				repelDist = 7;
		//			}
		//			//enemy.activated = false;
		//			//Vector3 center = tracker.placement.currentRow.sector.rows[repelDist].gameObject.GetComponent<Collider2D>().bounds.center;
		//			//enemy.mover.StartMovement(MoveType.Detour, repelSpeed, new Vector3(center.x, center.y));
		//			//enemy.mover.RedirectPath(new Vector3(center.x, center.y), repelSpeed);
		//			//enemy.mover.moving = true;
		//		}
		//	}
		//}
	}

	IEnumerator Unpopping(float recoverTime)
	{
		yield return new WaitForSeconds(recoverTime);
		tracker.unitGraphics.anim.SetTrigger("Recover");
		unpopping = false;
		popped = false;
		yield return null;

	}

}

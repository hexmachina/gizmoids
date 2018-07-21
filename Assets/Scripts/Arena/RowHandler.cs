using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RowHandler : MonoBehaviour
{

	public int place;
	public int aisle;

	//public bool occupied = false;
	//public Transform bottom;
	public SpriteRenderer renderer;
	//public Dictionary<string, List<ArenaPlacement>> groupsDict;

	//public List<List<GameObject>> groups;
	private List<GameObject> occupiers;
	//public List<ArenaPlacement> placers;
	//public SectorHandler sector;

	public List<string> warningTags = new List<string>();

	public List<SpriteRenderer> renderers = new List<SpriteRenderer>();

	public bool warning;

	Sequence sequence;

	// Use this for initialization
	void Start()
	{
		//aisle = sector.place;

		renderer = GetComponentInChildren<SpriteRenderer>();
		if (renderer)
			renderer.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (warningTags.Contains(collision.gameObject.tag) && !warning)
		{
			WarningSignal(5);
		}
	}

	public void WarningSignal(int loop)
	{
		sequence = DOTween.Sequence();
		renderer.color = Color.clear;
		renderer.gameObject.SetActive(true);

		sequence.Append(renderer.DOColor(new Color(1, 0, 0, 0.5f), 0.5f))
			.Append(renderer.DOColor(Color.clear, 0.5f))
			.SetLoops(loop);
	}

	//void FixedUpdate()
	//{
	//CleanUpOccupiers();
	//CheckWarning();
	//}

	//void OnTriggerEnter2D(Collider2D coll)
	//{
	//	//print("Row Collide " + coll.gameObject.tag);
	//	if (!occupiers.Contains(coll.gameObject))
	//	{
	//		foreach (string item in sector.rowWhiteList)
	//		{
	//			if (coll.gameObject.tag == item)
	//			{
	//				AddOccupier(coll.gameObject);
	//			}
	//		}
	//		CleanUpOccupiers();
	//	}
	//}

	//void OnTriggerStay2D(Collider2D coll)
	//{
	//	if (!occupiers.Contains(coll.gameObject))
	//	{
	//		foreach (string item in sector.rowWhiteList)
	//		{
	//			if (coll.gameObject.tag == item)
	//			{
	//				AddOccupier(coll.gameObject);
	//			}
	//		}
	//		CleanUpOccupiers();
	//	}
	//}

	//void OnTriggerExit2D(Collider2D coll)
	//{

	//}

	//public List<T> GetOccupiers<T>()
	//{
	//	var occs = new List<T>();
	//	for (int i = 0; i < occupiers.Count; i++)
	//	{
	//		var t = occupiers[i].GetComponent<T>();
	//		if (t != null)
	//		{
	//			occs.Add(t);
	//		}
	//	}
	//	return occs;
	//}

	//public List<GameObject> FindOccupiers(string s)
	//{
	//	List<GameObject> occu = new List<GameObject>();
	//	foreach (var item in occupiers)
	//	{
	//		if (item != null)
	//		{
	//			if (item.tag == s)
	//			{
	//				occu.Add(item);
	//			}
	//		}
	//	}
	//	return occu;
	//}

	public bool IsOccupiedBy(string s)
	{
		bool isHere = false;
		foreach (var item in occupiers)
		{
			if (item != null)
			{
				if (item.tag == s)
				{
					isHere = true;
					break;
				}
				else
				{
					isHere = false;
				}
			}
		}
		return isHere;
	}

	//bool CheckOccupation()
	//{
	//	if (occupiers.Count == 0)
	//	{
	//		occupied = false;
	//		sector.RemoveOccupiedRows(this);
	//	}
	//	else
	//	{
	//		occupied = true;
	//		sector.AddOccupiedRows(this);
	//	}
	//	return occupied;
	//}

	//void CleanUpOccupiers()
	//{
	//	List<GameObject> temp = new List<GameObject>(occupiers);
	//	for (var i = 0; i < temp.Count; i++)
	//	{
	//		if (!temp[i])
	//		{
	//			occupiers.Remove(temp[i]);
	//		}
	//	}

	//	CheckOccupation();
	//}

	//public void AddOccupier(GameObject go)
	//{
	//	var ap = go.GetComponent<ArenaPlacementLegacy>();
	//	if (ap)
	//	{
	//		ap.UpdatePlacement(this);
	//		occupiers.Add(go);
	//	}

	//	//go.BroadcastMessage("UpdatePlacement", this, SendMessageOptions.DontRequireReceiver);
	//}

	//public void RemoveOccupier(GameObject go)
	//{
	//	if (occupiers.Contains(go))
	//	{
	//		occupiers.Remove(go);
	//	}
	//	CleanUpOccupiers();
	//	CheckOccupation();
	//}

	public void ShowGraphics(float alpha = 1f)
	{
		if (alpha > 1f)
		{
			alpha = 1f;
		}
		if (alpha < 0)
		{
			alpha = 0;
		}
		if (renderer)
		{
			renderer.gameObject.SetActive(true);
			renderer.color = new Color(1, 1, 1, 1);
			renderer.color = new Color(1, 1, 1, alpha);
		}
	}

	public void HideGraphics()
	{
		if (renderer)
		{
			renderer.color = new Color(1, 1, 1, 1);
			renderer.gameObject.SetActive(false);
		}
	}

	IEnumerator FadeGraphics(float speed)
	{
		if (renderer)
		{
			//renderer.material.color = new Color(1, 1, 1, )
			while (renderer.color.a > 0)
			{
				renderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(renderer.color.a, 0, speed * Time.deltaTime));
				yield return null;
			}
			HideGraphics();
		}

		yield return null;
	}

	public void StartFadeGraphics(float alpha, float speed)
	{
		HideGraphics();
		ShowGraphics(alpha);
		StopAllCoroutines();
		StartCoroutine(FadeGraphics(speed));
	}

	//public void CheckWarning()
	//{
	//	if (ArenaHandler.instance == null)
	//	{
	//		return;
	//	}
	//	if (place == 0)
	//	{
	//		if (ArenaHandler.instance.sectors[aisle].rows[1].IsOccupiedBy("Enemy Point") && !ArenaHandler.instance.sectors[aisle].rows[1].IsOccupiedBy("Gizmoid Point") && !IsOccupiedBy("Gizmoid Point"))
	//		{

	//			ArenaHandler.instance.sectors[aisle].StartWarning();
	//			ArenaHandler.instance.sectors[aisle].rows[1].StartWarning();
	//			StartWarning();

	//		}
	//		else if (IsOccupiedBy("Enemy Point") && !IsOccupiedBy("Gizmoid Point"))
	//		{
	//			ArenaHandler.instance.sectors[aisle].StartWarning();
	//			ArenaHandler.instance.sectors[aisle].rows[1].StopWarning();
	//			StartWarning();
	//		}
	//		else
	//		{
	//			if (warning)
	//			{
	//				StopWarning();
	//				ArenaHandler.instance.sectors[aisle].rows[1].StopWarning();
	//				ArenaHandler.instance.sectors[aisle].StopWarning();

	//			}
	//		}
	//	}

	//	if (place == 7)
	//	{
	//		if (IsOccupiedBy("Enemy Point"))
	//		{
	//			if (!warning)
	//			{
	//				StartCoroutine(WarningTimer(2.8f));
	//			}
	//		}
	//		else
	//		{
	//			warning = false;
	//		}
	//	}
	//}

	//public IEnumerator WarningTimer(float seconds)
	//{
	//	warning = true;
	//	ShowGraphics();
	//	StartCoroutine(PhaseGraphics(2, Color.red, 0, 0.5f));
	//	yield return new WaitForSeconds(seconds);
	//	//StopCoroutine(PhaseGraphics(2, Color.red, 0, 0.5f));
	//	HideGraphics();
	//	StopAllCoroutines();
	//}

	//public void StartWarning()
	//{
	//	if (!warning)
	//	{
	//		StopAllCoroutines();
	//		ShowGraphics();
	//		warning = true;
	//		StartCoroutine(PhaseGraphics(2, Color.red, 0, 0.5f));
	//	}

	//}

	//public void StopWarning()
	//{
	//	StopAllCoroutines();
	//	HideGraphics();
	//	warning = false;
	//}

	public IEnumerator PhaseGraphics(float speed, Color color, float startAlpha, float endAlpha)
	{
		int phase = 0;
		float rate = speed;

		float max;
		float min;

		if (renderer)
		{
			if (startAlpha > endAlpha)
			{
				max = endAlpha;
				min = startAlpha;
				renderer.color = new Color(color.a, color.g, color.b, min);

			}
			else
			{
				max = startAlpha;
				min = endAlpha;
				renderer.color = new Color(color.a, color.g, color.b, max);
			}

			while (true)
			{
				if (phase % 2 == 0)
				{
					// print(sp.material.color.a);
					if (renderer.color.a > max)
					{
						renderer.color = new Color(color.a, color.g, color.b, Mathf.MoveTowards(renderer.color.a, max, rate * Time.deltaTime));
					}
					else
					{
						phase++;
						//print("alpha " + renderer.material.color.a + " max " + max + " phase " + phase);
					}
				}
				else
				{
					if (renderer.color.a < min)
					{
						renderer.color = new Color(color.a, color.g, color.b, Mathf.MoveTowards(renderer.color.a, min, rate * Time.deltaTime));
					}
					else
					{
						phase++;
						// print("alpha " + renderer.material.color.a + " min " + min + " phase " + phase);

					}
				}
				yield return null;
			}
		}
	}

}

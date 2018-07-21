using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectorHandler : MonoBehaviour
{

	//public ArenaHandler arena;
	[SerializeField]
	private List<RowHandler> rows = new List<RowHandler>();
	//public int place;
	//public bool occupied;

	//public bool warning;

	public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start()
	{
		if (rows.Count == 0)
		{
			GetRows();

		}
		if (!spriteRenderer)
		{
			var graph = transform.Find("Graphics");
			spriteRenderer = graph.gameObject.GetComponent<SpriteRenderer>();
		}
	}

	public List<RowHandler> GetRows()
	{
		rows.Clear();
		foreach (var item in GetComponentsInChildren<RowHandler>())
		{
			rows.Add(item);
		}
		return rows;
	}

	// Update is called once per frame
	//void Update()
	//{

	//}

	//void OnTriggerEnter2D(Collider2D coll)
	//{
	//	foreach (var item in sectorWhiteList)
	//	{
	//		if (coll.gameObject.tag == item)
	//		{
	//			//coll.BroadcastMessage("SetCurrentAisle", this);
	//			var ap = coll.GetComponent<ArenaPlacementLegacy>();
	//			if (ap)
	//			{
	//				ap.SetCurrentAisle(this);
	//			}
	//			//if (coll.gameObject.tag == "Blade Point")
	//			//{
	//			//    print(coll.gameObject.transform.parent.name + " " + place);

	//			//    currentBlade = coll.gameObject.GetComponentInParent<BladeBehaviourScript>();
	//			//    CheckBlades();


	//			//}
	//		}
	//	}
	//}

	//void OnTriggerStay2D(Collider2D coll)
	//{
	//}

	//void OnTriggerExit2D(Collider2D coll)
	//{

	//}

	//public void CheckBlades()
	//{
	//    foreach (var sector in ArenaHandler.instance.sectors)
	//    {
	//        if (sector.place != place)
	//        {
	//            if (sector.currentBlade != null)
	//            {
	//                if (sector.currentBlade.gameObject.GetInstanceID() == currentBlade.gameObject.GetInstanceID())
	//                {
	//                    sector.currentBlade = null;
	//                }
	//            }

	//        }

	//    }
	//}

	//public void AddOccupiedRows(RowHandler ro)
	//{
	//	if (!occupiedRows.Contains(ro))
	//	{
	//		occupiedRows.Add(ro);
	//	}

	//	CheckOccupation();
	//}

	//public void RemoveOccupiedRows(RowHandler ro)
	//{
	//	if (occupiedRows.Contains(ro))
	//	{
	//		occupiedRows.Remove(ro);
	//	}
	//	CheckOccupation();
	//}

	//public bool CheckOccupation()
	//{
	//	if (occupiedRows.Count == 0)
	//	{
	//		return occupied = false;
	//	}
	//	else
	//	{
	//		return occupied = true;
	//	}
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
		if (spriteRenderer)
		{
			spriteRenderer.gameObject.SetActive(true);
			spriteRenderer.color = new Color(1, 1, 1, alpha);
		}
	}

	public void HideGraphics()
	{
		if (spriteRenderer)
		{
			spriteRenderer.color = new Color(1, 1, 1, 1);
			spriteRenderer.gameObject.SetActive(false);
		}
	}

	IEnumerator FadeGraphics(float speed)
	{
		if (spriteRenderer)
		{
			//renderer.material.color = new Color(1, 1, 1, )
			while (spriteRenderer.color.a > 0)
			{
				spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(spriteRenderer.color.a, 0, speed * Time.deltaTime));
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



		if (spriteRenderer)
		{
			if (startAlpha > endAlpha)
			{
				max = endAlpha;
				min = startAlpha;
				spriteRenderer.color = new Color(color.a, color.g, color.b, min);

			}
			else
			{
				max = startAlpha;
				min = endAlpha;
				spriteRenderer.color = new Color(color.a, color.g, color.b, max);
			}

			while (true)
			{
				if (phase % 2 == 0)
				{
					// print(sp.material.color.a);
					if (spriteRenderer.color.a > max)
					{
						spriteRenderer.color = new Color(color.a, color.g, color.b, Mathf.MoveTowards(spriteRenderer.color.a, max, rate * Time.deltaTime));
					}
					else
					{
						phase++;
						//print("alpha " + renderer.material.color.a + " max " + max + " phase " + phase);
					}
				}
				else
				{
					if (spriteRenderer.color.a < min)
					{
						spriteRenderer.color = new Color(color.a, color.g, color.b, Mathf.MoveTowards(spriteRenderer.color.a, min, rate * Time.deltaTime));
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

	public void StartPhaseSector()
	{
		StartCoroutine(PhaseSector());
	}

	public IEnumerator PhaseSector()
	{
		print("Phasing Sector!");
		ShowGraphics();
		StartCoroutine(PhaseGraphics(0.3f, new Color(1, 1, 1), 0.2f, 0.4f));
		//yield return new WaitForSeconds(0.25f);
		for (int i = 0; i < rows.Count; i++)
		{
			int lastRow = i - 1;
			if (lastRow < 0)
			{
				lastRow = rows.Count - 1;
			}
			//rows[lastRow].HideGraphics();
			rows[i].ShowGraphics();
			if (i % 2 == 0)
			{
				StartCoroutine(rows[i].PhaseGraphics(0.3f, new Color(1, 1, 1), 0.4f, 0.2f));
			}
			else
			{
				StartCoroutine(rows[i].PhaseGraphics(0.3f, new Color(1, 1, 1), 0.2f, 0.4f));
			}
			//yield return new WaitForSeconds(0.25f);
		}
		yield return null;
	}

	public void ClearPhaseSector()
	{
		StopAllCoroutines();
		HideGraphics();
		foreach (var item in rows)
		{
			item.StopAllCoroutines();
			item.HideGraphics();
		}
	}

}

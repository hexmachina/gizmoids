using UnityEngine;
using System.Collections;

public class Hourglass : Spawn
{
	UnitGraphics animationManager;
	public float duration;
	public Color color;

	public GameObject finalBlast;

	public void StartHourglass(UnitGraphics am, float dur, Color col)
	{
		animationManager = am;
		color = col;
		duration = dur;


		StartCoroutine(HandleHourglass(duration, color));

	}

	public IEnumerator HandleHourglass(float seconds, Color col)
	{
		print("time " + seconds);
		animationManager.ShowSustainedColor(col);
		yield return new WaitForSeconds(seconds);
		animationManager.HideSusainedColor();
		print("Hourglass over");
		var blast = Instantiate(finalBlast) as GameObject;
		//blast.transform.eulerAngles = new Vector3(-90, 90, tracker.placement.currentAisle.transform.eulerAngles.z);
		blast.transform.position = transform.position;
		blast.GetComponent<Renderer>().sortingLayerName = "Projectiles";
		Destroy(gameObject);
	}
}

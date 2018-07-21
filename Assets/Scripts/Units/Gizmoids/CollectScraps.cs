using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectScraps : MonoBehaviour
{

	public bool activated;
	public bool activatedLastFrame = false;
	public float attractSpeed;
	//public CheckArena checkArena;
	public GizmoidView tracker;
	public List<Scrap> scraps;

	public bool overclocked;
	// Use this for initialization
	public AudioClip audioClip;

	void Start()
	{
		//tracker.overclocking.onOverclockChanged.AddListener(OnOverClocking);
	}

	// Update is called once per frame
	void Update()
	{
		if (!overclocked)
		{
			Attract();
			//tracker.animationManager.anim.SetBool("Active", checkArena.hasCompany);

			//if (checkArena.hasCompany)
			//{
			//	activated = true;
			//}
			//else
			//{
			//	activated = false;
			//}

			//if (!activatedLastFrame && activated)
			//{
			//	AudioPlayer.Instance.PlaySoundClip(tracker.assets.clipUtility1, transform);
			//}
			//activatedLastFrame = activated;
		}


	}

	void FixedUpdate()
	{
	}

	void SetActivation(bool f)
	{
		activated = f;
	}

	void GatherScraps()
	{

		//foreach (var item in checkArena.GetListeningRows())
		//{
		//	foreach (var scrap in item.FindOccupiers("Scrap Point"))
		//	{

		//		//print(this.name + " scrap " + scrap.tag);

		//		var parent = scrap.GetComponentInParent(typeof(Scrap)) as Scrap;
		//		if (parent.attracted == false)
		//		{
		//			//var newScrap = scrap as Scrap;
		//			parent.attracted = true;
		//			parent.attractor = transform;
		//			parent.StartAttraction();

		//			scraps.Add(parent);
		//		}

		//	}
		//}
	}

	void DropScraps()
	{
		foreach (var item in scraps)
		{
			if (item != null)
			{
				//item.EndAttraction();

			}

		}
		scraps.Clear();
	}

	void Attract()
	{
		//if (checkArena.CheckAisle())
		//{
		//	GatherScraps();
		//	for (var i = 0; i < scraps.Count; i++)
		//	{
		//		if (scraps[i] != null)
		//		{
		//			scraps[i].attracted = true;
		//			scraps[i].transform.position = Vector3.MoveTowards(scraps[i].transform.position, transform.position, attractSpeed * Time.deltaTime);
		//			//AudioPlayer.Instance.PlaySound(audioClip, transform);


		//		}
		//		else
		//		{
		//			scraps.Remove(scraps[i]);
		//		}
		//	}
		//}
		//else
		//{
		//	DropScraps();
		//}
	}

	//public void BeginOverclocking()
	//{
	//	//print("overclocking");
	//	overclocked = true;
	//	StopAllCoroutines();
	//	var allScraps = CollectibleSpawner.Instance.scraps;
	//	StartCoroutine(Overclocking(allScraps));
	//}

	public void OnOverClocking(bool clock)
	{
		overclocked = clock;
		StopAllCoroutines();
		if (overclocked)
		{
			var allScraps = CollectibleSpawner.Instance.scrapPool;
			StartCoroutine(Overclocking(allScraps));
		}
	}

	public IEnumerator Overclocking(List<Scrap> all)
	{
		AudioPlayer.Instance.PlaySoundClip(tracker.assets.clipUtility1, transform);
		while (all.Count > 0 && overclocked)
		{
			tracker.unitGraphics.anim.SetBool("Active", true);
			//for (int i = 0; i < all.Count; i++)
			//{
			//	if (all[i] != null)
			//	{
			//		all[i].attracted = true;
			//		all[i].transform.position = Vector3.MoveTowards(all[i].transform.position, transform.position, (attractSpeed * 1.4f) * Time.deltaTime);
			//	}
			//	else
			//	{
			//		all.RemoveAt(i);
			//	}
			//}
			var list = new List<Scrap>(all);
			//foreach (var item in list)
			//{
			//	if (item != null)
			//	{
			//		item.attracted = true;
			//		item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, (attractSpeed * 1.4f) * Time.deltaTime);
			//	}
			//	else
			//	{
			//		all.RemoveAt(list.IndexOf(item));
			//	}
			//}
			yield return null;
		}
		tracker.unitGraphics.anim.SetBool("Active", false);
		//tracker.overclocking.ForceStopOverclock();
		//tracker.health.HideOverclocking();
		overclocked = false;
		yield return null;
	}

}

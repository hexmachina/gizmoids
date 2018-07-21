using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GizmoidHolder : MonoBehaviour
{
	public class GizmoidEvent : UnityEvent<GizmoidView> { }

	public float spacing = 0.555f;

	public List<GizmoidView> gizmoids = new List<GizmoidView>(); //Current gizmoids in aisle

	public GameObject gizmoidPrefab;

	public GizmoidView prefabGizmoid;

	public BladeView blade;

	public GizmoidEvent onGizmoidAdded = new GizmoidEvent();

	private Sequence sequence;

	public void PlacePreview(GizmoidData data, UnitGraphics preview)
	{
		float duration = 0.5f;
		preview.transform.SetParent(transform);
		Vector3 rot = Vector3.zero;
		if (transform.localEulerAngles.z > 180f)
		{
			rot = new Vector3(0, 0, 359.9f);
		}
		preview.SetSortingLayer("Gizmoid" + gizmoids.Count);

		sequence = DOTween.Sequence();
		sequence.Join(preview.transform.DOLocalMove(new Vector3(0, blade.radiusOffset + (GetSpacing() * gizmoids.Count), 0), duration).SetEase(DG.Tweening.Ease.OutBounce))
			.Join(preview.transform.DOLocalRotate(rot, duration).SetEase(DG.Tweening.Ease.OutQuart))
			.OnComplete(() => BuildGizmoid(data, preview));
	}

	public float GetSpacing()
	{
		var space = spacing;
		var coll = gizmoidPrefab.GetComponent<BoxCollider2D>();
		if (coll)
		{
			space = coll.size.y;
		}
		return space;
	}

	public GizmoidView BuildGizmoid(GizmoidData data, UnitGraphics preview = null)
	{
		if (gizmoids.Count == 6)
		{
			return null;
		}

		var giz = Instantiate(prefabGizmoid, transform);
		giz.transform.localPosition = new Vector3(0, blade.radiusOffset + (GetSpacing() * gizmoids.Count), 0);
		giz.transform.eulerAngles = new Vector3(0, 0, blade.gameObject.transform.eulerAngles.z);
		//giz.holder = this;
		if (!preview)
		{
			preview = Instantiate(data.unitGraphics);
		}
		giz.SetData(data, gizmoids.Count, preview);
		gizmoids.Add(giz);
		onGizmoidAdded.Invoke(giz);
		giz.onDestroy.AddListener(RemoveGizmoid);

		var rand = Random.Range(0, giz.assets.clipArrivals.Count);
		AudioPlayer.Instance.PlaySoundClip(giz.assets.clipArrivals[rand], giz.transform);
		return giz;
	}

	public void RemoveGizmoid(EntityView entity)
	{
		//FXManager.instance.AddExplosion(giz.transform.collider2D.bounds.center);
		var giz = entity as GizmoidView;
		if (gizmoids.Contains(giz))
		{
			int place = gizmoids.IndexOf(giz);

			if (place != (gizmoids.Count - 1))
			{
				sequence = DOTween.Sequence();
				foreach (var item in gizmoids)
				{
					int index = gizmoids.IndexOf(item);
					if (index > place)
					{
						var newPos = blade.radiusOffset + (GetSpacing() * (index - 1));
						var shiftGiz = item;
						Debug.Log("new pos " + newPos);
						sequence.Join(shiftGiz.transform.DOLocalMoveY(newPos, 0.5f).SetEase(DG.Tweening.Ease.OutBounce)).AppendCallback(() => shiftGiz.OrderChanged(index - 1));
					}
				}
			}
			gizmoids.Remove(giz);
		}
	}
}

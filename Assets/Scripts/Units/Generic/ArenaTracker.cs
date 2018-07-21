using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ArenaTracker : MonoBehaviour
{
	[System.Serializable]
	public class SectorEvent : UnityEvent<Collider2D>
	{
	}

	private Collider2D arenaCollider;

	public SectorHandler currentSector;

	public SectorEvent onEnterSector = new SectorEvent();

	private void Awake()
	{
		if (!arenaCollider)
		{
			arenaCollider = GetComponent<Collider2D>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		var sector = collision.GetComponent<SectorHandler>();
		if (sector && currentSector != sector)
		{
			currentSector = sector;
			onEnterSector.Invoke(collision);
		}
	}

}

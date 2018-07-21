using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace GizLib
{
	[Serializable]
	public class UnityEventInt : UnityEvent<int>
	{

	}

	[Serializable]
	public class UnityEventIntValueChange : UnityEvent<IntValueChange>
	{

	}

	[Serializable]
	public class UnityEventBool : UnityEvent<bool>
	{

	}

	[Serializable]
	public class UnityEventFloat : UnityEvent<float>
	{

	}

	[Serializable]
	public class UnityEventVector3 : UnityEvent<Vector3>
	{

	}

	[Serializable]
	public class IntValueChange : EventArgs
	{
		public int value;
		public int lastValue;
		public int maxValue;
	}

	[Serializable]
	public class Path
	{
		public Vector2[] points;

		public Path(Vector2[] p)
		{
			points = p;
		}
	}

	[Serializable]
	public class TargetIntValue
	{
		public int targetValue;
		public int value;
		public TargetIntValue(int t)
		{
			targetValue = t;
			value = 0;
		}
	}

	public enum UtilityType
	{
		None = 0,
		FireBullet = 1,
		FireLaser = 2,
		CollectScrap = 3,
		RepelOnContact = 4,
		GenerateScrap = 5,
		RepairGizmoid = 6,
		SoundWave = 7,
		FreezeTime = 8,
		FireBubble = 9,
	}

	public enum CollectibleType
	{
		None = 0,
		BigScrap = 1,
		Blueprint = 2,
		Overclocker = 3,
		GizmoidSkin = 4,
		Xeno = 5,
		SpaceCrystal = 6,
		Crate = 7,
		GizMod = 8,
		XenoBronze = 9,
		XenoSilver = 10,
		XenoGold = 11,
		Scrap = 12
	}

	public enum BladeType
	{
		Default = 0,
		Rocket = 1,
		Luxury = 2
	}

	public class Helper
	{
		public static Vector2 GetScreenPoint()
		{
			Vector2 screenPoint = Vector2.zero;
			if (Input.touchSupported && Input.touchCount > 0)
			{
				screenPoint = Input.GetTouch(0).position;
			}
			else if (Input.mousePresent)
			{
				screenPoint = Input.mousePosition;
			}
			else
			{
				Debug.Log("wha happened?");
			}
			return screenPoint;
		}

		public static Vector3 GetWorldPoint()
		{
			return Camera.main.ScreenToWorldPoint(GetScreenPoint());
		}


	}
}

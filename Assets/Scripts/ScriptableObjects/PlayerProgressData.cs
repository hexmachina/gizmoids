using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using GizLib;

[CreateAssetMenu(menuName = "Data/Player Progress")]
public class PlayerProgressData : ScriptableObject
{
	[Serializable]
	public class IntEvent : UnityEvent<int> { }

	[Serializable]
	public class StringListEvent : UnityEvent<List<string>> { }

	[SerializeField]
	private int _scraps;
	public int Scraps
	{
		get
		{
			return _scraps;
		}
		set
		{
			SetProperty(ref _scraps, value);
		}
	}

	[SerializeField]
	[Range(0, 4)]
	private int _overclockTokens;
	public int OverclockTokens
	{
		get
		{
			return _overclockTokens;
		}
		set
		{
			SetProperty(ref _overclockTokens, value);
		}
	}

	public float overclockDuration = 4;

	[SerializeField]
	private int _xenos;
	public int Xenos
	{
		get
		{
			return _xenos;
		}
		set
		{
			SetProperty(ref _xenos, value);
		}
	}

	[SerializeField]
	private List<string> featuresUnlocked = new List<string>();

	public List<int> unlockedGizmoids = new List<int>();

	public IntEvent onScrapChanged = new IntEvent();

	public IntEvent onXenoChanged = new IntEvent();

	public IntEvent onOverclockChanged = new IntEvent();

	public StringListEvent onFeatureChanged = new StringListEvent();


	protected void SetProperty<T>(ref T currentValue, T newValue)
	{
		if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			return;
		currentValue = newValue;
	}

	public void AdjustScrap(int value)
	{
		var old = _scraps;
		_scraps += value;
		if (_scraps < 0)
		{
			_scraps = 0;
		}
		if (old != _scraps)
		{
			onScrapChanged.Invoke(_scraps);
		}
	}

	public void SetScraps(int value)
	{
		var old = _scraps;

		if (old != value)
		{
			_scraps = value;
			onScrapChanged.Invoke(_scraps);
		}
	}

	public void AdjustXeno(int value)
	{
		var old = _xenos;
		_xenos += value;
		_xenos = Mathf.Max(0, _xenos);
		if (old != _xenos)
		{
			onXenoChanged.Invoke(_xenos);
		}
	}

	public void SetXeno(int value)
	{
		var old = _xenos;

		if (old != value)
		{
			_xenos = value;
			onXenoChanged.Invoke(_xenos);
		}
	}

	public void AdjustOverclock(int value)
	{
		var old = _overclockTokens;
		_overclockTokens += value;
		_overclockTokens = Mathf.Max(0, _overclockTokens);
		_overclockTokens = Mathf.Min(_overclockTokens, 4);
		if (old != _overclockTokens)
		{
			onOverclockChanged.Invoke(_overclockTokens);
		}
	}

	public void SetOverclock(int value)
	{
		var old = _overclockTokens;
		_overclockTokens = Mathf.Max(0, _overclockTokens);
		_overclockTokens = Mathf.Min(_overclockTokens, 4);
		if (old != value)
		{
			_overclockTokens = value;
			onOverclockChanged.Invoke(_overclockTokens);
		}
	}

	public void SetUnlockedFeatures(List<string> features)
	{
		featuresUnlocked = new List<string>(features);
		onFeatureChanged.Invoke(featuresUnlocked);
	}

	public void AddFeatures(string feature, bool invoke)
	{
		var old = featuresUnlocked.Count;

		if (!featuresUnlocked.Contains(feature))
		{
			featuresUnlocked.Add(feature);
		}
		if (invoke && featuresUnlocked.Count != old)
		{
			onFeatureChanged.Invoke(featuresUnlocked);
		}
	}

	public void AddFeatures(params string[] features)
	{
		var old = featuresUnlocked.Count;
		foreach (var item in features)
		{
			if (!featuresUnlocked.Contains(item))
			{

				featuresUnlocked.Add(item);
			}
		}
		if (featuresUnlocked.Count != old)
		{
			onFeatureChanged.Invoke(featuresUnlocked);
		}
	}

	public List<string> GetUnlockedFeatures()
	{
		return featuresUnlocked;
	}

	public void Reset()
	{

		OverclockTokens = 2;
		Xenos = 0;
		Scraps = 0;
		featuresUnlocked.Clear();
		unlockedGizmoids.Clear();
	}

	public void OnCollected(CollectibleData data)
	{
		switch (data.collectibleType)
		{
			case CollectibleType.None:
				break;
			case CollectibleType.BigScrap:
			case CollectibleType.Scrap:
				AdjustScrap(data.value);
				break;
			case CollectibleType.Blueprint:
				unlockedGizmoids.Add(data.value);
				break;
			case CollectibleType.Overclocker:
				AdjustOverclock(data.value);
				break;
			case CollectibleType.GizmoidSkin:
				break;
			case CollectibleType.Xeno:
			case CollectibleType.XenoBronze:
			case CollectibleType.XenoSilver:
			case CollectibleType.XenoGold:
				AdjustXeno(data.value);
				break;
			case CollectibleType.SpaceCrystal:
				break;
			case CollectibleType.Crate:
				break;
			case CollectibleType.GizMod:
				break;
			default:
				break;
		}
	}
}

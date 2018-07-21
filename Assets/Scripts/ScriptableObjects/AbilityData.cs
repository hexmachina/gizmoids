using GizLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Unit/Ability")]
public class AbilityData : ScriptableObject
{

	public Ability abilityPrefab;

	public GameObject spawn;

	public Vector3 position;

	public List<string> targetTags = new List<string>();

	public List<AugumentStringFloat> keyValues = new List<AugumentStringFloat>();

	public Dictionary<string, float> GetDictionary()
	{
		var dict = new Dictionary<string, float>();

		foreach (var item in keyValues)
		{
			dict.Add(item.key, item.value);
		}
		return dict;
	}

	public Ability BuildAbility(EntityView entityView, AudioAssetData audioAsset)
	{
		var ability = Instantiate(abilityPrefab, entityView.transform);
		ability.name = abilityPrefab.name;
		ability.transform.localPosition = position;
		if (ability is ColliderAbility)
		{
			((ColliderAbility)ability).targetTags = targetTags;
		}

		if (ability is IAbility)
		{
			((IAbility)ability).SetAudioAsset(audioAsset);
			((IAbility)ability).SetFields(GetDictionary());
		}

		if (ability is IListenable)
		{
			((IListenable)ability).AddGraphicsListener(entityView.unitGraphics);
		}

		if (ability is ILastActionable)
		{
			((ILastActionable)ability).AddLastActionListener(() => entityView.SelfDestruct(false));
		}

		if (ability is ISpawnable)
		{
			((ISpawnable)ability).AttachSpawn(spawn);
		}
		return ability;
	}
}

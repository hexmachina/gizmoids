using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{

	void SetFields(Dictionary<string, float> valuePairs);

	void SetAudioAsset(AudioAssetData audioAsset);
}

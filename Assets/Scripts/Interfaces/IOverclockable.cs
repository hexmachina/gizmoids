using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOverclockable
{
	void SetOverclock(bool active);

	void Overclocking(float duration);
}

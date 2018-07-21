using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable
{

	void SetPaused(bool isPaused);

	void Pause(float duration);
}

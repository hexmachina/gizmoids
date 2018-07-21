using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GizLib
{
	public interface IAcquirable
	{
		int Price { get; set; }
		float Cooldown { get; set; }
		Sprite Icon { get; set; }
	}
}


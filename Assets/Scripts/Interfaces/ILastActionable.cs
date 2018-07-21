using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILastActionable
{
	void AddLastActionListener(System.Action callback);

}

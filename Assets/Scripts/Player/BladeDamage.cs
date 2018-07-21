using UnityEngine;
using System.Collections;

public class BladeDamage : MonoBehaviour
{

	public void HandleImpact(BladeView blade, GameObject gameObject)
	{
		if (blade.bladeData.spriteStates.Count > 1)
		{
			blade.spriteRenderer.sprite = blade.bladeData.spriteStates[1];
		}
		blade.GetComponent<Collider2D>().enabled = false;
		blade.enabled = false;

		var en = gameObject.GetComponent<EntityView>();
		if (!en && gameObject.transform.parent != null)
		{
			en = gameObject.GetComponentInParent<EntityView>();
		}
		if (en)
		{
			en.SelfDestruct();
		}
	}
}

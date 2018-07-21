using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesOnDestroy : Ability
{

	public List<EnemyData> spawn = new List<EnemyData>();


	private void Start()
	{
		var health = GetComponent<Health>();

		if (!health)
		{
			health = GetComponentInParent<Health>();
		}
		if (health)
		{
			health.onHealthChange.AddListener(OnHealthChange);
		}
	}

	private void OnHealthChange(GizLib.IntValueChange intValue)
	{


		if (intValue.value <= 0 && EnemySpawner.Instance)
		{
			for (int i = 0; i < spawn.Count; i++)
			{
				var distance = Vector3.Distance(transform.position, Vector3.zero) + (0.555f * 2.5f);
				var en = EnemySpawner.Instance.SpawnEnemy(spawn[i], transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z));
				float offset = 45;
				if (i % 2 == 0)
				{
					offset *= -1f;
					en.transform.localScale = new Vector3(en.transform.localScale.x * -1, en.transform.localScale.y, en.transform.localScale.z);
				}
				var degree = transform.eulerAngles.z + offset + 90;
				var target = Mathfx.GetPointOnCircle(distance, degree, Vector3.zero);
				en.mover.StartMovement(MoveType.Detour, en.mover.speed * 20, target);
				en.mover.onDetour.AddListener(en.unitGraphics.OnActivateTrigger);
				en.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + offset);

			}
		}
	}

}

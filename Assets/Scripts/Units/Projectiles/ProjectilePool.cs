using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{

	private static ProjectilePool _instance;

	public static ProjectilePool Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<ProjectilePool>();
				if (_instance == null)
				{
					var go = new GameObject("Projectile Pool");
					_instance = go.AddComponent<ProjectilePool>();
				}
			}
			return _instance;
		}
	}

	public List<Projectile> pool = new List<Projectile>();

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			if (this != _instance)
			{
				Destroy(gameObject);
			}
		}
	}

	public Projectile GetProjectile(int id)
	{
		for (int i = 0; i < pool.Count; i++)
		{
			if (pool[i].spawnId == id && !pool[i].gameObject.activeSelf)
			{
				return pool[i];
			}
		}

		return null;
	}

	public void AddProjectile(Projectile projectile)
	{
		if (projectile.transform.parent != transform)
		{
			projectile.transform.SetParent(transform);
		}

		if (!pool.Contains(projectile))
		{
			pool.Add(projectile);
		}
	}
}

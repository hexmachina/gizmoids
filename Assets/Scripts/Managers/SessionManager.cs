using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{

	private static SessionManager _instance;
	public static SessionManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<SessionManager>();
				if (_instance == null)
				{
					var go = new GameObject("Session Manager");
					_instance = go.AddComponent<SessionManager>();
				}
			}
			return _instance;
		}
	}

	public int levelToLoad = -1;

	public LevelData levelData;

	public List<int> gizmoidLoadout = new List<int>();

	void Awake()
	{
		if (_instance == null)
		{
			Debug.Log("Instance is null");
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			if (this != _instance)
			{
				Debug.Log("SM instance destroyed @" + Time.frameCount);
				Destroy(gameObject);
			}
		}
	}
}

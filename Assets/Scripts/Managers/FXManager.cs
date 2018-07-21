using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXManager : MonoBehaviour {
    public static FXManager instance;
    public GameObject pointer;
    public List<GameObject> explosions;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddExplosion(Vector3 point)
    {
        int rand = Random.Range(0, explosions.Count - 1);
        GameObject expl = Instantiate(explosions[rand], point, Quaternion.identity) as GameObject;
        expl.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        expl.transform.parent = transform;
    }


    public void AddPointer(Vector3 point)
    {
        GameObject p = Instantiate(pointer, point, Quaternion.identity) as GameObject;
        p.transform.parent = transform;
    }
}

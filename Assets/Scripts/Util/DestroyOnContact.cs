using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

    public string[] blackList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        foreach (string item in blackList)
        {
            if (coll.gameObject.tag == item)
            {
                Destroy(coll.gameObject);
            }
        }

    }
}

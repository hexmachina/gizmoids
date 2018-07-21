using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public int power = 0;
    //public ArenaPlacement placement;
    public float firingRate;
    public GameObject laserHit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Gizmoid":
                print("LASER!");
                
                SpawnLaserHit(coll.gameObject.transform.position);
                coll.gameObject.BroadcastMessage("TakeDamage", power);
                StopAllCoroutines();
                StartCoroutine(FiringPhase(firingRate));
                break;
        }
    }

    //void OnCollisionEnter2D(Collision2D coll)
    //{
    //    print(coll.gameObject.tag);
    //    switch (coll.gameObject.tag)
    //    {
    //        case "Gizmoid":
    //            print("LASER!");
    //            gameObject.collider2D.enabled = false;
    //            SpawnLaserHit(new Vector2(coll.contacts[0].point.x, coll.contacts[0].point.y));
    //            coll.gameObject.BroadcastMessage("TakeDamage", power);
    //            StartCoroutine(FiringPhase(firingRate));
    //            break;
    //    }
    //}

    IEnumerator FiringPhase(float sec)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(sec);
        gameObject.GetComponent<Collider2D>().enabled = true;
        yield return null;
    }

    void Impact(Health h)
    {
        h.TakeDamage(power);
    }

    void SelfDestruct()
    {
        //placement.ClearFromRow(placement.gameObject);
        Destroy(gameObject);
    }

    void SpawnLaserHit(Vector3 point)
    {
        GameObject hit = Instantiate(laserHit, point, Quaternion.identity) as GameObject;
        hit.transform.eulerAngles = transform.eulerAngles;
    }
    
}

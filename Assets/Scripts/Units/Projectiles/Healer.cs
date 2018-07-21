using UnityEngine;
using System.Collections;

public class Healer : MonoBehaviour {

    public string sortingLayer;

    public ParticleSystem hearts;
    public ParticleSystem plus;
    public ParticleSystem gears;

	// Use this for initialization
	void Start () {
        //EnableParticles(false, false, false);
        hearts.GetComponent<Renderer>().sortingLayerName = sortingLayer;
        plus.GetComponent<Renderer>().sortingLayerName = sortingLayer;
        gears.GetComponent<Renderer>().sortingLayerName = sortingLayer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnableParticles(bool heart, bool pl, bool gear)
    {
        hearts.enableEmission = heart;
        plus.enableEmission = pl;
        gears.enableEmission = gear;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Blueprint : MonoBehaviour {

    public SpriteRenderer outlineRenderer;

    public List<Sprite> outlines;

    public int currentOutline;
	// Use this for initialization
	void Start () {
        //outlineRenderer = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //setOutline(currentOutline);
	}

    public void setOutline(int i)
    {
        outlineRenderer.sprite = outlines[i];
    }
}

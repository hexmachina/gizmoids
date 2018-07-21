using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGParrallax : MonoBehaviour {

    public List<GameObject> bgList;
    public float initialSpeed;
    public float scrollSpeed;
    public Vector2 textScale;
    private float distance = 0;
    private SpriteRenderer sRenderer;
    public bool mask;
    public string layer;
    public int order;

	// Use this for initialization
	void Start () {
        //print(gameObject.renderer.sharedMaterial.shader.name);
        textScale = gameObject.GetComponent<Renderer>().sharedMaterial.GetTextureScale("_MainTex");

        if (mask)
        {
            gameObject.GetComponent<Renderer>().sortingLayerName = layer;
            gameObject.GetComponent<Renderer>().sortingOrder = order;
        }

        resetSpeed();
	}
	
	// Update is called once per frame
	void Update () {

        //Move(scrollSpeed);
	}

    public void Move(float speed, float rot)
    {
        //distance += Time.time * speed;
        var currentOffset = GetOffset();
            //gameObject.renderer.sharedMaterial.GetTextureOffset("_MainTex");
        
        float y = Mathf.Repeat(currentOffset.y + Direction(Time.deltaTime * speed, rot).y, 1);
        float x = Mathf.Repeat(currentOffset.x + Direction(Time.deltaTime * speed, rot).x, 1);

        //float y = Time.time * Direction(speed).y;
        //print(y);
        //print("BG " + gameObject.GetInstanceID() +" "+ y);
        Vector2 offset = new Vector2(x, y);

        SetOffset(offset);
        //gameObject.renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    public Vector2 Direction(float speed, float rot)
    {
        Vector2 offset = Vector2.zero;
        //var rot = PlayerContolScript.instance.gameObject.transform.eulerAngles.z;
        //print(rot);
        if (rot >= 0 && rot <= 90)
        {
            float y = speed - (speed * (rot / 90));
            float x = -(speed * (rot / 90));
            offset = new Vector2(x, y);
        }
        else if (rot > 90 && rot <= 180)
        {
            rot -= 90;
            float x = -(speed - (speed * (rot / 90)));
            float y = -(speed * (rot / 90));
            offset = new Vector2(x, y);
        }
        else if (rot > 180 && rot <= 270)
        {
            rot -= 180;
            float y = -(speed - (speed * (rot / 90)));
            float x = (speed * (rot / 90));
            offset = new Vector2(x, y);
        }
        else if (rot > 270 && rot <= 360)
        {
            rot -= 270;
            float x = (speed - (speed * (rot / 90)));
            float y = (speed * (rot / 90));
            offset = new Vector2(x, y);
        }
        else if (rot < 0 && rot >= -90)
        {
            rot *= 1;
            float x = (speed * (rot / 90));
            float y = speed - (speed * (rot / 90));
            offset = new Vector2(x, y);
        }
        return offset;
    }

    public void resetSpeed()
    {
        scrollSpeed = initialSpeed;
    }

    public void SetOffset(Vector2 offset)
    {
        if (sRenderer)
        {
            sRenderer.material.SetTextureOffset("_MainTex", offset);
        }
        else
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }

    public Vector2 GetOffset()
    {
        Vector2 offset;
        if (sRenderer)
        {
            offset = sRenderer.material.GetTextureOffset("_MainTex");
        }
        else
        {
            offset = gameObject.GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
        }
        return offset;
    }

    void OnDisable()
    {

            gameObject.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", Vector2.zero);
    }
}

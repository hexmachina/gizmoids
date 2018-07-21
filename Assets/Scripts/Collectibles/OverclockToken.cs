using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverclockToken : MonoBehaviour {
    public List<Color> rainbow;
    public List<SpriteRenderer> renderers;
	// Use this for initialization
	void Start () {
        StartCoroutine(OverclockingPhase());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator OverclockingPhase()
    {
        int phase = 0;
        float rate = 4;
        List<string> cop = new List<string>();
        cop.Add("equals");
        cop.Add("equals");
        cop.Add("equals");
        cop.Add("equals");

        while (true)
        {

            if (phase < rainbow.Count)
            {
                int nextColor = phase + 1;
                if (nextColor >= rainbow.Count)
                {
                    nextColor = 0;
                }
                cop[0] = CheckOperator(rainbow[phase].r, rainbow[nextColor].r);
                cop[1] = CheckOperator(rainbow[phase].g, rainbow[nextColor].g);
                cop[2] = CheckOperator(rainbow[phase].b, rainbow[nextColor].b);
                cop[3] = CheckOperator(rainbow[phase].a, rainbow[nextColor].a);
                foreach (var item in renderers)
                {
                    item.material.color = Mathfx.ColorMoveTowards(item.material.color, rainbow[phase], rate * Time.deltaTime);
                }
                //print("blue " + renderers[renderers.Count - 1].material.color.b);
                if (CheckColors(CheckDiff(renderers[renderers.Count - 1].material.color.r, rainbow[phase].r, cop[0]), CheckDiff(renderers[renderers.Count - 1].material.color.g, rainbow[phase].g, cop[1]), CheckDiff(renderers[renderers.Count - 1].material.color.b, rainbow[phase].b, cop[2]), CheckDiff(renderers[renderers.Count - 1].material.color.a, rainbow[phase].a, cop[3])))
                {
                    //print("phasing");
                    phase++;
                }
            }
            else
            {
                phase = 0;
            }
            yield return null;
        }
        //yield return null;

    }
    public string CheckOperator(float a, float b)
    {
        string check = "equals";
        if (a == b)
        {
            check = "equals";
        }
        else if (a < b)
        {
            check = "less";
        }
        else if (a > b)
        {
            check = "more";
        }

        return check;
    }

    public bool CheckDiff(float a, float b, string check)
    {
        bool foo = false;
        switch (check)
        {
            case "equals":
                if (a == b)
                {
                    foo = true;
                }
                break;
            case "less":
                if (a <= b)
                {
                    foo = true;
                }
                break;
            case "more":
                if (a >= b)
                {
                    foo = true;
                }
                break;
        }
        return foo;
    }

    public bool CheckColors(bool r, bool g, bool b, bool a)
    {
        bool check = false;
        if (r && g && b && a)
        {
            check = true;
        }
        return check;
    }


    //public void ActionStart()
    //{
    //    PlayerResources.Instance.AddOverclockToken();
    //}

}

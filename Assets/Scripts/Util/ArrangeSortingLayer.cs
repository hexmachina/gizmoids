using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrangeSortingLayer : MonoBehaviour {

    //public string sortingLayer;
    public List<UnitGraphics> animationManagers;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OrderAnimationManagers(UnitGraphics[] am)
    {
        int tier = 0;

        for (int i = 0; i < am.Length; i++)
        {

            if (i - 1 > -1)
            {
                tier += am[i - 1].highestOrder;
                am[i].level = tier;
                tier += 3;
            }
            foreach (var item in am[i].renderers)
            {
                item.sortingOrder = am[i].initialSort[am[i].renderers.IndexOf(item)] + tier;
            }
        }
    }

    public void CheckandReorder(){
        OrderAnimationManagers(GetComponentsInChildren<UnitGraphics>());
    }

    public void ReorderLayer()
    {
        int highestOrder = 0;
        var renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (var item in renderers)
        {
            highestOrder += item.sortingOrder;
            item.sortingOrder = highestOrder;
            highestOrder++;
        }
    }

    public void AddToOrder(GameObject go)
    {
        int highestOrder = 0;
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        highestOrder = renderers.Length;
        var rends = go.GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in renderers)
        {
            highestOrder += item.sortingOrder;
            item.sortingOrder = highestOrder;

        }
    }
}

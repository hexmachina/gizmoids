using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour {

    public static BackgroundManager instance;

    public List<Background> backgrounds;

    public Background currentBackground;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        currentBackground.MoveBackgrounds();
    }

    public void SetCurrentBackground(int id)
    {
        if (currentBackground == null)
        {
            print("world id " + id);
            var curr = backgrounds.Find(x => x.worldId == id);
            var bg = Instantiate(curr) as Background;
            bg.transform.parent = transform;
            bg.transform.localPosition = Vector3.zero;
            currentBackground = bg;
        }
    }
}

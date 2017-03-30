using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameSet : MonoBehaviour 
{
    public int gameMode, maxValueOnTiles;
    public string operations;
    public bool timeIsRunningOut;

    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("NotToDestroy");
        if (gos.Length == 0)
        {
            tag = "NotToDestroy";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            int i = 0;
            do
            {
                if (gos[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
                else
                {
                    tag = "NotToDestroy";
                    DontDestroyOnLoad(gameObject);
                }
                i++;
            } while(i < gos.Length);
        }
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet : MonoBehaviour 
{
    public int gameMode, timeMode, maxValueOnTiles;
    public string operations;

    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("NotToDestroy");

        foreach (GameObject go in gos)
        {
            if (go.name == gameObject.name)
            {
                Destroy(gameObject);
            }
            else
            {
                tag = "NotToDestroy";
                DontDestroyOnLoad(gameObject);
            }
        }
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour 
{
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

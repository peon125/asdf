using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour 
{
    public int value;

	void Start()
    {
        GameControl gameController = GameObject.Find("gameController").GetComponent<GameControl>();
        GetComponent<Button>().onClick.AddListener(() => gameController.TileTouched(gameObject));
	}
}

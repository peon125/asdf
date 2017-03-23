using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxValueOnTilesSlider : MonoBehaviour 
{
    public Text text;
    public GameSet gameSet;

	void Update() 
    {
        int max = (int)GetComponent<Slider>().value;
        text.text = max.ToString();
        gameSet.maxValueOnTiles = max;
	}
}

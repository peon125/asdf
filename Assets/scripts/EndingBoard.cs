using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EndingBoard : MonoBehaviour 
{
    public Text pointsText, secodnsText;
    public GameControl gameController;

	void Start() 
    {
        pointsText.text = gameController.counterText.text;
        secodnsText.text = gameController.GetTime().ToString();
	}

    public void Quit()
    {
        SceneManager.LoadScene("menu01");
    }
}
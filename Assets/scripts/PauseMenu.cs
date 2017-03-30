using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class PauseMenu : MonoBehaviour 
{
    public Color[] backgroundColors;
    public Image background;
    int colorCounter;
    bool isPaused;

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

        colorCounter = 0;
        isPaused = false;
	}

    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            PauseTheGame();
        }
    }

    public void PauseTheGame()
    {
        Time.timeScale = 1 - Time.timeScale;

        isPaused = !isPaused;

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("uibutton"))
        {
            button.GetComponent<Button>().interactable = !isPaused;
        }

        transform.GetChild(0).gameObject.SetActive(isPaused);
    }
    
    public void ChangeBackgroundColor()
    {
        colorCounter++;
        colorCounter %= backgroundColors.Length;
        background.color = backgroundColors[colorCounter];
    }

    public void BackToTheMenu()
    {
        SceneManager.LoadScene("menu01");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu02Control : MonoBehaviour 
{
    public GameObject borderPrefab;
    public GameSet gameSet;
    public Image gameModeImage, timeModeImage;
    int gameModeCounter, timeModeCounter;

    public void ChangeGameMode()
    {
        gameModeCounter++;
        gameModeCounter %= 3;

        switch (gameModeCounter)
        {
            case 0:
                gameModeImage.color = new Color(1, 1, 0);
                gameModeImage.transform.GetChild(0).GetComponent<Text>().text = "turn to zeros";
                break;
            case 1:
                gameModeImage.color = new Color(1, 0, 1);
                gameModeImage.transform.GetChild(0).GetComponent<Text>().text = "delete tiles";
                break;
            case 2:
                gameModeImage.color = new Color(0, 1, 1);
                gameModeImage.transform.GetChild(0).GetComponent<Text>().text = "infinite mode";
                break;
        }

        gameSet.gameMode = gameModeCounter;
    }

    public void ChangeTimeMode()
    {
        timeModeCounter++;
        timeModeCounter %= 2;

        switch(timeModeCounter)
        {
            case 0:
                timeModeImage.color = Color.green;
                timeModeImage.transform.GetChild(0).GetComponent<Text>().text = "time passes";
                break;
            case 1:
                timeModeImage.color = Color.red;
                timeModeImage.transform.GetChild(0).GetComponent<Text>().text = "time doesn't pass";
                break;
        }

        gameSet.timeMode = timeModeCounter;
    }
}

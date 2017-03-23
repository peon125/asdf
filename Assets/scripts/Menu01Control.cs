using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu01Control : MonoBehaviour 
{
    public void GoFuther()
    {
        SceneManager.LoadScene("menu02");
    }

    public void QuitTheGame()
    {
        //xD;
    }
}

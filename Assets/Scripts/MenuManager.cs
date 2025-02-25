using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject credits;
    public int mainGameSceneIndex = 1;
    public void ShowCredits()
    { 

        credits.SetActive(!credits.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(mainGameSceneIndex);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(mainGameSceneIndex);
    }




}

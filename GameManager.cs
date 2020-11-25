using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static GameDelegate GamePaused;
    public static GameDelegate GameContinou;

    
    public GameObject Bird_one;
    public GameObject Bird_two;
    public GameObject Bird_three;
    public GameObject PausePage;
    public GameObject GameOverPage;
    public int MinLevelPoint;
    public Text GameOverText;
    public GameObject NextLevelButton;
    public Text WarningText;
    
    private bool PauseControl = false;

    

    private void OnEnable()
    {
        ScoreCount.GameOver += GameOver;
    }

    private void OnDisable()
    {
        ScoreCount.GameOver -= GameOver;
    }


    public void LoadScene_1()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }

        if (GameOverPage != null)
        {
            GameOverPage.SetActive(false);
        }
        SceneManager.LoadScene("Level1");
    }
    public void LoadScene_2()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }
        if (GameOverPage != null)
        {
            GameOverPage.SetActive(false);
        }
        SceneManager.LoadScene("Level2");
    }
    public void LoadScene_3()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }
        if (GameOverPage != null)
        {
            GameOverPage.SetActive(false);
        }
        SceneManager.LoadScene("Level3");
    }
    public void LoadScene_4()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }
        if (GameOverPage != null)
        {
            GameOverPage.SetActive(false);
        }
        SceneManager.LoadScene("Level4");
    }
    public void LoadScene_5()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }
        if (GameOverPage != null)
        {
            GameOverPage.SetActive(false);
        }
        SceneManager.LoadScene("Level5");
    }
    public void LoadMenu()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }
        if (GameOverPage != null)
        {
            GameOverPage.SetActive(false);
        }
        SceneManager.LoadScene("menuPage");
    }
    public void LoadLevels()
    {
        if (PausePage != null)
        {
            PausePage.SetActive(false);
        }
        SceneManager.LoadScene("LevelsPage");
        
    }


    public void GamePause()
    {
        GamePaused();
        PausePage.SetActive(true);
    }

    public void GameContinous()
    {
        PausePage.SetActive(false);
        GameContinou();
    }


    public void GameOver(int totalScore)
    {
        GameOverPage.SetActive(true);
        GameOverText.text = totalScore.ToString();
        if (totalScore < MinLevelPoint)
        {
            WarningText.enabled = true;
            if (NextLevelButton != null)
            {
                NextLevelButton.SetActive(false);
            }
        }
    }
}

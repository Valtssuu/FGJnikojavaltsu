﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject pausePanel;

    

    public void LevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        

    }
    public void TestLevel()
    {
        SceneManager.LoadScene("TestLevel");
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}

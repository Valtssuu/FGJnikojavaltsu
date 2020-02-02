using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject detonateButton;
    

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
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void Level4()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void Level5()
    {
        SceneManager.LoadScene("Level 5");
    }
    public void Level6()
    {
        SceneManager.LoadScene("Level 6");
    }
    public void Level7()
    {
        SceneManager.LoadScene("Level 7");
    }
    public void Level8()
    {
        SceneManager.LoadScene("Level 8");
    }
    public void Level9()
    {
        SceneManager.LoadScene("Level 9");
    }
    public void Level10()
    {
        SceneManager.LoadScene("Level 10");
    }
    public void Level11()
    {
        SceneManager.LoadScene("Level 11");
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Detonate()
    {
        detonateButton.SetActive(false);
    }

}

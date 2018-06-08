using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public GameObject ingameGUI;
    public GameObject pauseMenu;
    public GameObject gameManager;

    public void OnClickLevelSelect()
    {
        SceneManager.LoadScene("levelSelect");
        Time.timeScale = 1;
    }

    public void OnClickOptions()
    {
        //SceneManager.LoadScene("Options");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("0");
    }

    public void OnClickResume()
    {
        pauseMenu.active = false;
        Time.timeScale = 1;
        gameManager.GetComponent<GameManager>().paused = false;
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}

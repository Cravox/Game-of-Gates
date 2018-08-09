using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public GameObject ingameGUI;
    public GameObject pauseMenu;
    public GameManager gameManager;

    public void OnClickLevelSelect()
    {
        SceneManager.LoadScene("levelSelect");
        Time.timeScale = 1;
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
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

    public void ClickSP()
    {
        SceneManager.LoadScene("LoadingScreen_SP");
        Time.timeScale = 1;

    }

    public void ClickMP()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LoadingScreen_MP");

    }

    public void ClickSelectGate()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("levelSelect_Gate2");
    }

    public void OnClickCredits()
    {
        SceneManager.LoadScene("credits");
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Tutorial");
    }
}

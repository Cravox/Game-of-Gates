using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class GameManager : MonoBehaviour
{
    public bool paused = true;
    public GameObject player1;
    public GameObject player2;
    public GameObject pauseMenu;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject Jussi;
    public GameObject bossCountdown;
    public AudioSource mainTheme;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene("levelSelect");
        }

        if(Jussi != null)
        {
            if(Jussi.GetComponent<Jussi>().hp <= 0)
            {
                winScreen.active = true;
                paused = true;
            }
        }

        if(Input.GetButtonDown("Pause"))
        {
            if(paused)
            {
                if(mainTheme != null)
                {
                    mainTheme.volume = 0.3f;
                }
                //Time.timeScale = 1;
                pauseMenu.active = false;
                paused = false;
            }else
            {
                if (mainTheme != null)
                {
                    mainTheme.volume = 0.05f;
                }
                GamePad.SetVibration(0, 0, 0);
                //Time.timeScale = 0;
                pauseMenu.active = true;
                paused = true;
            }
        }

        if(player1 == null || player1 == null && player2 == null)
        {
            paused = true;
            loseScreen.active = true;
        }
    }
}

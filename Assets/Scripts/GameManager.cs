using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;
    public GameObject pauseMenu;
    public AudioSource mainTheme;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene("levelSelect");
        }

        if(Input.GetButtonDown("Pause"))
        {
            if(paused)
            {
                mainTheme.volume = 0.3f;
                Time.timeScale = 1;
                pauseMenu.active = false;
                paused = false;
            }else
            {
                mainTheme.volume = 0.05f;
                Time.timeScale = 0;
                pauseMenu.active = true;
                paused = true;
            }
        }


    }
}

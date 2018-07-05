using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class GameManager : MonoBehaviour
{
    public bool paused = false;
    public bool noInput = true;
    public bool multiPlayer;
    private bool allDead = false;
    private bool[] blinkStates = new bool[2];

    public Sprite[] playerHpUISprites = new Sprite[5];

    public Image[] HpImages;
    public GameObject[] Players;
    public GameObject pauseMenu;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject Jussi;
    public GameObject bossCountdown;
    public bool[] blinkings = new bool[2];

    public AudioSource mainTheme;
    private Player[] hpPlayers;

    void Start()
    {
        Time.timeScale = 1;
        hpPlayers = new Player[Players.Length];
        for (int i = 0; i < Players.Length; i++)
        {
            hpPlayers[i] = Players[i].GetComponent<Player>();
        }
    }

    void Update()
    {
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene("levelSelect_Gate2");
        }

        if (Jussi != null)
        {
            if (Jussi.GetComponent<Jussi>().hp <= 0)
            {
                winScreen.active = true;
                paused = true;
            }
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (paused)
            {
                if (mainTheme != null)
                {
                    mainTheme.volume = 0.3f;
                }
                //Time.timeScale = 1;
                pauseMenu.active = false;
                paused = false;
            }
            else
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

        for (int i = 0; i < Players.Length; i++)
        {
            switch (hpPlayers[i].hp)
            {
                case 2:
                    HpImages[i].sprite = playerHpUISprites[2];
                    break;
                case 1:
                    if (!blinkings[i])
                    {
                        StartCoroutine(HpBlink(HpImages[i], i));
                        blinkings[i] = true;
                    }
                    break;
                case 0:
                    HpImages[i].sprite = playerHpUISprites[4];
                    StopCoroutine(HpBlink(HpImages[i], i));
                    break;
            }
        }

        if(multiPlayer)
        {
            if (Players[0].activeSelf == false && Players[1].activeSelf == false)
            {
                paused = true;
                loseScreen.SetActive(true);
            }
        }
        else
        {
            if(Players[0].activeSelf == false)
            {
                paused = true;
                loseScreen.SetActive(true);
            }
        }
    }

    private IEnumerator HpBlink(Image hpImage, int playerID)
    {
        while (true)
        {
            if (blinkStates[playerID])
            {
                hpImage.sprite = playerHpUISprites[1];
            }
            else
            {
                hpImage.sprite = playerHpUISprites[0];
            }
            blinkStates[playerID] = !blinkStates[playerID];
            yield return new WaitForSeconds(0.5f);
        }
    }
}

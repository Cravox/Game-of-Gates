using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class GameManager : MonoBehaviour
{
    public AudioClip[] audioClips = new AudioClip[2];
    public AudioClip[] musicThemes = new AudioClip[3];
    public GameObject[] dialogue = new GameObject[5];
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
    private AudioSource audiosource;
    private Player[] hpPlayers;
    private float delay = 0f;
    private bool soundTriggered = false;

    void Start()
    {
        audiosource = this.GetComponent<AudioSource>();
        Time.timeScale = 1;
        hpPlayers = new Player[Players.Length];
        for (int i = 0; i < Players.Length; i++)
        {
            hpPlayers[i] = Players[i].GetComponent<Player>();
        }
    }

    void Update()
    {
        if (!noInput)
        {
            Jussi.GetComponent<Animator>().SetTrigger("DialogueFinished");
        }

        DialogueManager();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene("levelSelect_Gate2");
        }

        if (Jussi.GetComponent<Jussi>().dead == true)
        {
            if (Jussi.GetComponent<Jussi>().hp <= 0)
            {
                //allAudioSources[0].enabled = false;
                //allAudioSources[1].enabled = true;
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
                noInput = true;
                loseScreen.SetActive(true);
                if(!soundTriggered)
                {
                    audiosource.PlayOneShot(audioClips[Random.Range((int)0, (int)1)]);
                    soundTriggered = true;  
                }
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

    void DialogueManager()
    {
        if(noInput)
        {
            if(dialogue[0].activeSelf && Input.GetButtonDown("Jump_0"))
            {
                dialogue[0].SetActive(false);
                dialogue[1].SetActive(true);
            }
            else if(dialogue[1].activeSelf && Input.GetButtonDown("Jump_0"))
            {
                dialogue[1].SetActive(false);
                dialogue[2].SetActive(true);
            }
            else if (dialogue[2].activeSelf && Input.GetButtonDown("Jump_0"))
            {
                dialogue[2].SetActive(false);
                dialogue[3].SetActive(true);
            }
            else if (dialogue[3].activeSelf && Input.GetButtonDown("Jump_0"))
            {
                dialogue[3].SetActive(false);
                dialogue[4].SetActive(true);
            }
            if(dialogue[4].activeSelf)
            {
                delay += Time.deltaTime;
                print(delay);
                if(delay >= 1f)
                {
                    dialogue[4].SetActive(false);
                    bossCountdown.SetActive(true);
                }
            }
        }

    }
}

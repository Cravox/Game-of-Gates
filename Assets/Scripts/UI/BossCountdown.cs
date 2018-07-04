using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCountdown : MonoBehaviour
{
    public GameObject countdownPanel;
    public GameManager gameManager;
    public Sprite[] countDownSprites = new Sprite[4];
    public float counter = 1f;
    public int countLimit = 4;
    private Image countDownImage;


    void Start()
    {
        countDownImage = this.GetComponent<Image>();
    }

    void Update()
    {
        counter -= Time.deltaTime;

        if(counter <= 0)
        {
            countLimit -= 1;
            counter = 1f;
        }

        if (countLimit == 0)
        {
            countdownPanel.SetActive(false);
            gameManager.noInput = false;
            counter = Time.realtimeSinceStartup * 2;
        }

        if(countLimit >= 0)
        {
            countDownImage.sprite = countDownSprites[countLimit];
        }
    }
}

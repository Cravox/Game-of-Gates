using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCountdown : MonoBehaviour
{
    public GameObject countdownPanel;
    public GameManager gameManager;
    private Text countDown;
    public int countLimit = 3;
    private float counter = 2f;

    void Start()
    {
        countDown = this.GetComponent<Text>();
    }

    void Update()
    {
        if (Time.realtimeSinceStartup >= counter)
        {
            counter += 1.5f;
            countLimit -= 1;

            if (countLimit == 0)
            {
                gameManager.GetComponent<GameManager>().paused = false;
                countdownPanel.SetActive(false);
            }
        }
        countDown.text = countLimit + "...";
    }
}

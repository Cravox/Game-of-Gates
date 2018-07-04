using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltiMeter : MonoBehaviour
{
    private bool blinkState;
    private bool blinking = false;

    public Sprite[] ultiSprites;
    private Image ultiMeter;

    // Use this for initialization
    void Start()
    {
        ultiMeter = this.GetComponent<Image>();
        ultiSprites[0] = ultiMeter.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(ultiMeter.fillAmount == 1f && !blinking)
        {
            StartCoroutine("UltBlink");
            blinking = true;
        }
        else if (ultiMeter.fillAmount < 1f)
        {
            blinking = false;
            ultiMeter.sprite = ultiSprites[0];
            StopCoroutine("UltBlink");
        }
    }

    private IEnumerator UltBlink()
    {
        while (true)
        {
            if (blinkState)
            {
                ultiMeter.sprite = ultiSprites[0];
            }
            else
            {
                ultiMeter.sprite = ultiSprites[1];
            }
            blinkState = !blinkState;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

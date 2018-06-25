using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltiMeter : MonoBehaviour
{
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
        if(ultiMeter.fillAmount == 1)
        {
            ultiMeter.sprite = ultiSprites[1];
        }
        else
        {
            ultiMeter.sprite = ultiSprites[0];
        }
    }
}

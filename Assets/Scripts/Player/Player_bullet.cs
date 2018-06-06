﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_bullet : MonoBehaviour
{
    public int normalDamage;
    public float normalRange;
    public float normalUltiProfit;
    public int spreadDamage;
    public float spreadRange;
    public float spreadUltiProfit;

    private GameObject ultiMeter;
    private Image ultiMeterImage;
    private int damage;
    public int playerIndex;
    private float range;
    private float ultiProfit;



    void Start()
    {
        Destroy(this.gameObject, range);
        ultiMeter = GameObject.Find("Ultimeter_"+this.playerIndex);
        ultiMeterImage = ultiMeter.GetComponent<Image>();
    }
    
    void Update()
    {

    }

    public enum Type : int
    {
        NORMAL = 0,
        SPREAD
    }

    public void Initialize(Type type){
        switch (type)
        {
            case Type.NORMAL:
                damage = normalDamage;
                range = normalRange;
                ultiProfit = normalUltiProfit;
                break;
            case Type.SPREAD:
                damage = spreadDamage;
                range = spreadRange;
                ultiProfit = spreadUltiProfit;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            ultiMeterImage.fillAmount += ultiProfit;
            col.gameObject.GetComponent<Jussi>().hp -= damage;
            Destroy(this.gameObject);
        }

        if (col.gameObject.CompareTag("PeanutMissile"))
        {
            ultiMeterImage.fillAmount += ultiProfit;
            col.gameObject.GetComponent<Jussi_peanutMissile>().hp -= damage;
            Destroy(this.gameObject);
        }

        if(col.gameObject.CompareTag("Ballon"))
        {
            Destroy(this.gameObject);
        }
    }
}
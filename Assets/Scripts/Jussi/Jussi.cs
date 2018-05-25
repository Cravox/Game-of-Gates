﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi : MonoBehaviour
{

    public GameObject yoshiEgg;
    public GameObject peanutMissile;
    public GameObject flipNormalInstantiate;
    public GameObject circleLaserInstantiate;
    public Transform instantiate;
    public Transform yoshiEggSpawnPosition;
    public Transform peanutMissileSpawnPosition;
    public int hp;
    public int criticalPhaseTrigger = 400;

    public int yoshiEggNumber = 4;
    public float yoshiEggSpeed = 250f;
    public float yoshiEggFrequency = 1f;
    public float yoshiEggFastFrequency = 0.6f;

    public int peanutMissileNumber = 3;
    public float peanutMissileFrequency = 2;
    public float peanutMissileForce = 100f;

    public float flipNormalsLifeTime = 4f;

    public float circleLaserLifeTime = 4f;


    private Animator anim;
    private int attack = 1;
    private int critAttack = 1;
    private int yoshiEggCounter = 0;
    private int peanutMissileCounter = 0;
    private float circleLaserLifeTimeCounter = 0;
    private float flipNormalsLifeTimeCounter = 0;
    private float moveY;
    private float peanutMissileSpawnTimer = 0;
    private float shootTimer = 0;
    private bool defaultShot = true;
    private bool regularPhase = true;
    private bool firstPhase = true;
    private float attackDelay = 0;

    void Update()
    {
        if (this.hp <= 0)
        {
            Destroy(this.gameObject, 0.2f);
        }

        if (this.hp <= criticalPhaseTrigger)
        {
            regularPhase = false;
        }

        if (regularPhase)
        {
            switch (attack)
            {
                case 1:
                    AttackOne();
                    break;
                case 2:
                    AttackTwo();
                    break;
            }
        }
        else
        {
            switch (critAttack)
            {
                case 1:
                    CritAttackOne();
                    break;
                case 2:
                    CritAttackTwo();
                    break;
            }
        }
    }

    void AttackOne()
    {
        Vector3 spawn = yoshiEggSpawnPosition.transform.position;
        shootTimer += Time.deltaTime;
        if (shootTimer >= yoshiEggFrequency && yoshiEggCounter < yoshiEggNumber)
        {
            GameObject yoshiEggInstance = Instantiate(yoshiEgg, spawn, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
            yoshiEggCounter += 1;
            shootTimer -= yoshiEggFrequency;

            if (yoshiEggCounter == 4 && firstPhase)
            {
                firstPhase = false;
                yoshiEggCounter = 0;
                yoshiEggFrequency = yoshiEggFastFrequency;
                shootTimer = -1f;
            }

            if (yoshiEggCounter == 4 && !firstPhase)
            {
                shootTimer = 0;
                yoshiEggCounter = 0;
                attack = 2;
            }
        }
    }

    void AttackTwo()
    {
        shootTimer += Time.deltaTime;
        Vector3 spawn = peanutMissileSpawnPosition.transform.position;

        GameObject[] peanutMissiles = new GameObject[peanutMissileNumber];
        float missileAngle = -0.4f;
        if(shootTimer >= peanutMissileFrequency)
        {
            for(int i = 0; i < peanutMissiles.Length; i++)
            {
                peanutMissiles[i] = Instantiate(peanutMissile, spawn, Quaternion.identity);
                peanutMissiles[i].GetComponent<Rigidbody>().AddForce(new Vector3(missileAngle, 1, 0) * peanutMissileForce);
                missileAngle += 0.4f;
            }
            peanutMissileCounter += 1;
            shootTimer -= peanutMissileFrequency;
        }

        if(peanutMissileCounter == 2)
        {
            shootTimer = -2f;
            yoshiEggFrequency = 1;
            firstPhase = true;
            peanutMissileCounter = 0;
            attack = 1;
        }
    }

    void CritAttackOne()
    {
        flipNormalInstantiate.SetActive(true);
        flipNormalsLifeTimeCounter += Time.deltaTime;

        if(flipNormalsLifeTimeCounter >= flipNormalsLifeTime)
        {
            flipNormalInstantiate.SetActive(false);
            critAttack = 2;
            circleLaserLifeTimeCounter = -0.5f;
        }

    }

    void CritAttackTwo()
    {
        circleLaserInstantiate.SetActive(true);
        circleLaserLifeTimeCounter += Time.deltaTime;

        if(circleLaserLifeTimeCounter >= circleLaserLifeTime)
        {
            print("Lukas stinkt nach Serkan");
            circleLaserInstantiate.SetActive(false);
            critAttack = 1;
            flipNormalsLifeTimeCounter = -0.5f;
        }

    }
}
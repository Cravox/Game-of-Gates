using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi : MonoBehaviour
{

    public GameObject yoshiEgg;
    public GameObject peanutMissile;
    public GameObject flipNormal;
    public GameObject circleLaser;
    public Transform instantiate;
    public Transform yoshiEggSpawnPosition;
    public Transform peanutMissileSpawnPosition;
    public int hp;
    public int yoshiEggNumber = 4;
    public int peanutMissileNumber = 3;
    public float yoshiEggFrequency = 1f;
    public float peanutMissileForce = 100f;
    public float yoshiEggSpeed = 250f;

    private Animator anim;
    private int phase = 1;
    private int yoshiEggCounter = 0;
    private float moveY;
    private float shootTimer;
    private bool defaultAttack = true;

    void Awake()
    {

    }

    void Update()
    {
        if (this.hp <= 0)
        {
            Destroy(this.gameObject, 0.2f);
        }

        if (this.hp <= 400)
        {
            phase = 2;
        }

        switch (phase)
        {
            case 1:
                PhaseOne();
                break;
            case 2:
                PhaseTwo();
                break;
        }
    }

    void PhaseOne()
    {
        //float attackDelay = 0;
        //attackDelay += Time.deltaTime;

        if (defaultAttack)
        {
            shootTimer += Time.deltaTime;
            Vector3 spawn = yoshiEggSpawnPosition.transform.position;

            if (shootTimer >= yoshiEggFrequency && yoshiEggCounter < yoshiEggNumber)
            {
                GameObject yoshiEggInstance = Instantiate(yoshiEgg, spawn, Quaternion.identity);
                yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
                yoshiEggCounter += 1;

                if (yoshiEggCounter >= yoshiEggNumber)
                {
                    defaultAttack = false;
                }
                shootTimer -= yoshiEggFrequency;
            }
        }
        else
        {
            Vector3 spawn = peanutMissileSpawnPosition.transform.position;
            GameObject[] peanutMissiles = new GameObject[peanutMissileNumber];
            float missileAngle = -0.9f;
            {
                for (int i = 0; i < peanutMissiles.Length; i++)
                {
                    peanutMissiles[i] = Instantiate(peanutMissile, spawn, Quaternion.identity);
                    peanutMissiles[i].GetComponent<Rigidbody>().AddForce(new Vector3(missileAngle, 1, 0) * peanutMissileForce);

                    missileAngle += 0.9f;
                }
            }
            yoshiEggCounter = 0;
            defaultAttack = true;
        }

    }

    void PhaseTwo()
    {
        print("PHASE 2 STARTS");
    }
}

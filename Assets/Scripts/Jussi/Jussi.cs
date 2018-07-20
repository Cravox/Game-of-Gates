using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi : MonoBehaviour
{
    public AudioSource[] allAudioSources;
    public Animator mülleimer;
    public GameObject[] Lights = new GameObject[9];
    public GameObject[] bottles = new GameObject[7];
    public GameObject[] cracks = new GameObject[3];
    public GameObject mihawkSword;
    public GameObject yoshiEgg;
    public GameObject peanutMissile;
    public GameObject flipNormalInstantiate;
    public GameObject circleLaserInstantiate;
    public GameObject gameManagerObj;
    public SkinnedMeshRenderer[] renderer;
    public Transform yoshiEggSpawnPosition;
    public Transform peanutMissileSpawnPosition;
    public int hp;
    public int flipNormalTrigger = 1200;
    public int thirdPhaseTrigger = 400;

    public int yoshiEggNumber = 4;
    public float yoshiEggSpeed = 250f;
    public float yoshiEggFrequency = 1f;
    public float yoshiEggFastFrequency = 0.6f;

    public int peanutMissileNumber = 3;
    public float peanutMissileFrequency = 2;
    public float peanutMissileForce = 100f;

    public float flipNormalsLifeTime = 4f;

    public float circleLaserLifeTime = 4f;
    
    private GameManager gameManager;
    private Color originalColor;
    private Animator anim;
    private int phase = 1;
    //private int attack = 1;
    //private int critAttack = 1;
    private int yoshiEggCounter = 0;
    private int peanutMissileCounter = 0;
    private float circleLaserLifeTimeCounter = 0;
    private float flipNormalsLifeTimeCounter = 0;
    private float peanutMissileSpawnTimer = 0;
    private float shootTimer = 0;
    private bool blinking;
    private bool defaultShot = true;
    //private bool regularPhase = true;
    private bool firstPhase = true;
    private float attackDelay = 0;
    private int bossHpPercent;
    private float delay = 0;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        bossHpPercent = hp / 100;
        renderer = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer item in renderer)
        {
            originalColor = item.material.color;
        }
        allAudioSources = this.GetComponents<AudioSource>();
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    void Update()
    {
        if (this.hp <= 0)
        {
            Death();
        }

        if (this.hp <= flipNormalTrigger)
        {
            phase += 1;
        }

        if(this.hp <= thirdPhaseTrigger)
        {
            phase += 1;
        }

        if (!gameManager.GetComponent<GameManager>().paused && !gameManager.GetComponent<GameManager>().noInput)
        {
            switch(phase)
            {
                case 1:
                    PhaseOne();
                    break;
                case 2:
                    PhaseTwo();
                    break;
                case 3:
                    if (!blinking)
                    {
                        StartCoroutine("Flashing");
                        blinking = true;
                    }
                    PhaseThree();
                    break;
            }
        }

        if (gameManager.multiPlayer)
        {
            switch (hp)
            {
                case 2600:
                    cracks[0].SetActive(true);
                    for (int i = 0; i < bottles.Length; i++)
                    {
                        bottles[i].GetComponent<Rigidbody>().AddForce(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                    }
                    break;
                case 2300:
                    mülleimer.GetComponent<Animator>().enabled = true;
                    break;
                case 2100:
                    cracks[2].SetActive(true);
                    break;
                case 2000:
                    mihawkSword.GetComponent<Rigidbody>().AddForce(Random.Range(0, 10), 0, Random.Range(0, -20));
                    break;
                case 1600:
                    cracks[1].SetActive(true);
                    break;
            }
        }
        else
        {
            switch (hp)
            {
                case 1200:
                    cracks[0].SetActive(true);
                    for (int i = 0; i < bottles.Length; i++)
                    {
                        bottles[i].GetComponent<Rigidbody>().AddForce(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                    }
                    break;
                case 1000:
                    mülleimer.GetComponent<Animator>().enabled = true;
                    break;
                case 900:
                    mihawkSword.GetComponent<Rigidbody>().AddForce(Random.Range(0, 10), 0, Random.Range(0, -20));
                    break;
                case 600:
                    cracks[1].SetActive(true);
                    break;
                case 300:
                    cracks[2].SetActive(true);
                    break;
            }
        }
    }

    void PhaseOne()
    {
        YoshiEggsAttack();
    }

    void PhaseTwo()
    {

    }

    void PhaseThree()
    {

    }

    void YoshiEggsAttack()
    {
        Vector3 spawn = yoshiEggSpawnPosition.transform.position;
        shootTimer += Time.deltaTime;
        if (shootTimer >= yoshiEggFrequency && yoshiEggCounter < yoshiEggNumber)
        {
            allAudioSources[0].Play();

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
            }
        }
    }

    void PeanutStreamAttack()
    {
        shootTimer += Time.deltaTime;
        Vector3 spawn = peanutMissileSpawnPosition.transform.position;

        GameObject[] peanutMissiles = new GameObject[peanutMissileNumber];
        float missileAngle = -0.4f;
        if (shootTimer >= peanutMissileFrequency)
        {
            allAudioSources[1].Play();

            for (int i = 0; i < peanutMissiles.Length; i++)
            {
                peanutMissiles[i] = Instantiate(peanutMissile, spawn, Quaternion.identity);
                peanutMissiles[i].GetComponent<Rigidbody>().AddForce(new Vector3(missileAngle, 1, 0) * peanutMissileForce);
                missileAngle += 0.4f;
            }
            peanutMissile.GetComponent<Jussi_peanutMissile>().defaultTarget = !peanutMissile.GetComponent<Jussi_peanutMissile>().defaultTarget;
            peanutMissileCounter += 1;
            shootTimer -= peanutMissileFrequency;
        }

        if (peanutMissileCounter == peanutMissileFrequency)
        {
            shootTimer = -2f;
            yoshiEggFrequency = 1;
            firstPhase = true;
            peanutMissileCounter = 0;
        }
    }

    void FlipNormalsAttack()
    {
        delay += Time.deltaTime;
        if (delay >= 2f)
        {
            flipNormalInstantiate.SetActive(true);
            flipNormalsLifeTimeCounter += Time.deltaTime;
            if (flipNormalsLifeTimeCounter >= flipNormalsLifeTime)
            {
                flipNormalInstantiate.SetActive(false);
                circleLaserLifeTimeCounter = 0;
                delay = 0;
            }
        }
    }

    void LaserAttack()
    {
        delay += Time.deltaTime;
        if (delay >= 2f)
        {
            circleLaserInstantiate.SetActive(true);
            circleLaserLifeTimeCounter += Time.deltaTime;

            if (circleLaserLifeTimeCounter >= circleLaserLifeTime)
            {
                circleLaserInstantiate.SetActive(false);
                flipNormalsLifeTimeCounter = 0;
            }
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            foreach(SkinnedMeshRenderer item in renderer)
            {
                item.material.color = Color.white;
            }
            Invoke("ResetColor", 0.075f);
        }
    }

    void ResetColor()
    {
        foreach (SkinnedMeshRenderer item in renderer)
        {
            item.material.color = originalColor;
        }
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                Lights[Random.Range(0, Lights.Length)].SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++)
            {
                Lights[Random.Range(0, Lights.Length)].SetActive(false);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Death()
    {
        flipNormalInstantiate.SetActive(false);
        circleLaserInstantiate.SetActive(false);
        Destroy(this.gameObject, 1f);
    }
}
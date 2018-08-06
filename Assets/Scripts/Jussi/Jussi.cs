using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Jussi : MonoBehaviour
{
    public AudioSource[] allAudioSources;
    public Animator mülleimer;
    public Animator anim;
    public AnimationClip yoshiEggAnimation;
    public GameManager gameManager;
    public GameObject[] Lights = new GameObject[9];
    public GameObject[] bottles = new GameObject[7];
    public GameObject[] cracks = new GameObject[3];
    public GameObject mihawkSword;
    public GameObject yoshiEgg;
    public GameObject peanutMissile;
    public GameObject flipNormalInstantiate;
    public GameObject circleLaserInstantiate;
    public SkinnedMeshRenderer[] renderer;
    public Transform yoshiEggSpawnPosition;
    public Transform peanutMissileSpawnPosition;
    public int hp;
    public int flipNormalTrigger = 1200;
    public int thirdPhaseTrigger = 400;
    public float pauseTime = 1f;

    public int yoshiEggNumber = 4;
    public float yoshiEggSpeed = 250f;
    public float[] yoshiEggFrequencies = new float[3];
    public float[] yoshiEggAnimationMultipliers = new float[3];

    public int peanutMissileNumber = 3;
    public float peanutMissileFrequency = 2;
    public float peanutMissileForce = 100f;
    public bool animReady;

    public float flipNormalsLifeTime = 4f;

    public float circleLaserLifeTime = 4f;

    private Color originalColor;
    private int phase = 1;
    private int yoshiEggCounter = 0;
    private int peanutMissileCounter = 0;
    private int maxHp;
    private int yoshiEggPhase = 1;
    private float yoshiEggFrequency;
    private float circleLaserLifeTimeCounter = 0;
    private float flipNormalsLifeTimeCounter = 0;
    private float peanutMissileSpawnTimer = 0;
    private float delay = 0;
    private float attackDelay = 0;
    private float pauseTimer = 0;
    private float shootTimer;
    private bool blinking;
    private bool defaultShot = true;
    private bool firstPhase = true;
    private int bossHpPercent;

    private void Start()
    {
        anim.speed = yoshiEggAnimationMultipliers[0];
        anim = this.GetComponent<Animator>();
        bossHpPercent = hp / 100;
        renderer = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in renderer)
        {
            originalColor = item.material.color;
        }
        allAudioSources = this.GetComponents<AudioSource>();

        yoshiEggFrequency = yoshiEggFrequencies[0];

        maxHp = hp;

    }

    void Update()
    {
        if (this.hp <= 0)
        {
            Death();
        }

        if (this.hp <= flipNormalTrigger)
        {
            phase = 2;
        }

        if (this.hp <= thirdPhaseTrigger)
        {
            phase = 3;
        }

        if (!gameManager.paused && !gameManager.noInput)
        {
            switch (phase)
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
        } // is scheiße mach neu
          // is scheiße mach neu
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
        }                                                                             // is scheiße mach neu
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
        }                                                                                                     // is scheiße mach neu
    }

    void PhaseOne()
    {
        pauseTimer += Time.deltaTime;
        if (pauseTimer >= pauseTime)
        {
            YoshiEggsAttack();
            pauseTime -= pauseTime;
        }
    }

    void PhaseTwo()
    {
        FlipNormalsAttack();
    }

    void PhaseThree()
    {
        flipNormalInstantiate.SetActive(false);

        if (firstPhase)
        {
            PeanutStreamAttack();
        }
        else
        {
            LaserAttack();
        }
    }

    public void AnimTry()
    {
        animReady = true;
    }


    void YoshiEggsAttack()
    {
        anim.SetTrigger("YoshiAttack");
        if (animReady && yoshiEggCounter < yoshiEggNumber)
        {
            animReady = false;
            allAudioSources[0].Play();

            GameObject yoshiEggInstance = Instantiate(yoshiEgg, yoshiEggSpawnPosition.transform.position, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
            yoshiEggCounter += 1;


            if (yoshiEggCounter == 4)
            {
                switch (yoshiEggPhase)
                {
                    case 1:
                        yoshiEggAnimation. = yoshiEggAnimationMultipliers[0];
                        YoshiEggSwapPhase();
                        yoshiEggFrequency = yoshiEggFrequencies[1];
                        break;
                    case 2:
                        anim.speed = yoshiEggAnimationMultipliers[1];
                        YoshiEggSwapPhase();
                        yoshiEggFrequency = yoshiEggFrequencies[2];
                        break;
                    case 3:
                        anim.speed = yoshiEggAnimationMultipliers[2];
                        YoshiEggSwapPhase();
                        yoshiEggFrequency = yoshiEggFrequencies[0];
                        break;
                }
            }
        }

    }


    void YoshiEggSwapPhase()
    {
        shootTimer = -1f;
        yoshiEggCounter = 0;

        if (yoshiEggPhase != 3)
        {
            yoshiEggPhase += 1;
        }
        else
        {
            yoshiEggPhase = 1;
        }
    }

    void FlipNormalsAttack()
    {
        delay += Time.deltaTime;
        if (delay >= 2f)
        {
            flipNormalInstantiate.SetActive(true);
            //flipNormalsLifeTimeCounter += Time.deltaTime;
            //if (flipNormalsLifeTimeCounter >= flipNormalsLifeTime)
            //{
            //    flipNormalInstantiate.SetActive(false);
            //    circleLaserLifeTimeCounter = 0;
            //    delay = 0;
            //}
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
            firstPhase = false;
            peanutMissileCounter = 0;
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
                firstPhase = true;
            }
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            foreach (SkinnedMeshRenderer item in renderer)
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
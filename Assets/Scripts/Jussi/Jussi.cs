using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Jussi : MonoBehaviour
{
    public AudioSource[] allAudioSources;
    public Animator mülleimer;
    public Animator anim;
    public GameManager gameManager;
    public GameObject[] bottles = new GameObject[7];
    public GameObject[] cracks = new GameObject[3];
    public GameObject mihawkSword;
    public GameObject yoshiEgg;
    public GameObject peanutMissile;
    public GameObject flipNormalInstantiate;
    public GameObject circleLaserInstantiate;
    public SkinnedMeshRenderer[] renderer;
    public Transform yoshiEggSpawnPosition;
    public Transform[] peanutMissileSpawnPosition = new Transform[2];
    public int hp;
    public int flipNormalTrigger = 1200;
    public int thirdPhaseTrigger = 400;
    public float pauseTime = 1f;
    public bool flipNormals = false;
    public bool firstPhase = true;

    public float yoshiEggSpeed = 250f;
    public int yoshiEggPhase = 0;

    public int peanutMissileNumber = 3;
    public float peanutMissileFrequency = 2;
    public float peanutMissileForce = 100f;
    public float flipNormalsLifeTime = 4f;
    public float circleLaserLifeTime = 4f;

    public bool animReady;

    private Color originalColor;
    private int phase = 1;
    private int yoshiEggCounter = 0;
    private int peanutMissileCounter = 0;
    private int maxHp;
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
    private bool triggered = false;
    private int bossHpPercent;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        bossHpPercent = hp / 100;
        renderer = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in renderer)
        {
            originalColor = item.material.color;
        }
        allAudioSources = this.GetComponents<AudioSource>();

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
            anim.SetTrigger("Rage");
            flipNormalInstantiate.SetActive(false);
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
        anim.SetBool("YoshiAttack", true);
        switch (yoshiEggPhase)
        {
            case 1:
                YoshiEggsAttack();
                break;
            case 2:
                YoshiEggsAttackSec();
                break;
            case 3:
                YoshiEggsAttackThird();
                break;
        }
    }

    void PhaseTwo()
    {
        if(!triggered)
        {
            anim.SetBool("YoshiAttack", false);
            anim.SetTrigger("Transition");
            triggered = true;
        }

        if(flipNormals) FlipNormalsAttack();
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

    void YoshiEggPhaseSwap()
    {
        if(yoshiEggPhase < 3)
        {
            yoshiEggPhase += 1;
        }
        else
        {
            yoshiEggPhase = 1;
        }
    }

    public void AnimTry()
    {
        animReady = true;
    }

    void YoshiEggsAttack()
    {
        anim.SetBool("YoshiAttack1", true);
        anim.SetBool("YoshiAttack2", false);
        anim.SetBool("YoshiAttack3", false);

        if (animReady)
        {
            animReady = false;
            allAudioSources[0].Play();

            GameObject yoshiEggInstance = Instantiate(yoshiEgg, yoshiEggSpawnPosition.transform.position, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
        }

    }

    void YoshiEggsAttackSec()
    {
        anim.SetBool("YoshiAttack1", false);
        anim.SetBool("YoshiAttack2", true);
        anim.SetBool("YoshiAttack3", false);

        if (animReady)
        {
            animReady = false;
            allAudioSources[0].Play();

            GameObject yoshiEggInstance = Instantiate(yoshiEgg, yoshiEggSpawnPosition.transform.position, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
        }

    }

    void YoshiEggsAttackThird()
    {
        anim.SetBool("YoshiAttack1", false);
        anim.SetBool("YoshiAttack2", false);
        anim.SetBool("YoshiAttack3", true);

        if (animReady)
        {
            animReady = false;
            allAudioSources[0].Play();

            GameObject yoshiEggInstance = Instantiate(yoshiEgg, yoshiEggSpawnPosition.transform.position, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
        }

    }

    void FlipNormalsAttack()
    {
        anim.SetTrigger("FlipNormals");
        flipNormalInstantiate.SetActive(true);
    }

    void PeanutStreamAttack()
    {
        anim.SetTrigger("Peanut");
        shootTimer += Time.deltaTime;

        bool firstInstantiate = true;

        Vector3[] spawns = new Vector3[2];

        for(int i = 0; i < 1; i++)
        {
            spawns[i] = peanutMissileSpawnPosition[i].transform.position;
        }

        GameObject[] peanutMissiles = new GameObject[peanutMissileNumber];
        float missileAngle = -0.4f;
        if (shootTimer >= peanutMissileFrequency)
        {
            allAudioSources[1].Play();

            for (int i = 0; i < peanutMissiles.Length; i++)
            {
                peanutMissiles[i] = Instantiate(peanutMissile, spawns[0], Quaternion.identity);
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
        anim.SetTrigger("CircleLaser");
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

    void Death()
    {
        anim.SetTrigger("Death");
        flipNormalInstantiate.SetActive(false);
        circleLaserInstantiate.SetActive(false);
    }
}
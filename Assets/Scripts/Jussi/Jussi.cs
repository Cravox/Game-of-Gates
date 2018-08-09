using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Jussi : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips = new AudioClip[16];
    public Texture[] textures = new Texture[2];
    public Animator mülleimer;
    public Animator anim;
    public GameManager gameManager;
    public GameObject camera;
    public GameObject[] jussiBody = new GameObject[5];
    public GameObject[] bottles = new GameObject[7];
    public GameObject[] cracks = new GameObject[3];
    public GameObject mihawkSword;
    public GameObject[] yoshiEggs = new GameObject[3];
    public GameObject peanutMissile;
    public GameObject flipNormalInstantiate;
    public GameObject circleLaserInstantiate;
    public GameObject impact;
    public SkinnedMeshRenderer[] renderer;
    public Transform yoshiEggSpawnPosition;
    public Transform[] peanutMissileSpawnPosition = new Transform[2];
    public int hp;
    public int phase = 1;
    public int flipNormalTrigger = 1200;
    public int thirdPhaseTrigger = 400;
    public float pauseTime = 1f;
    public bool flipNormals = false;
    public bool peanutPhase = true;
    public bool laserAttackDone = false;
    public bool peanutAttackDone = false;
    public bool dead = false;
    public bool won = false;

    public float yoshiEggSpeed = 250f;

    public int peanutMissileNumber = 3;
    public float peanutMissileFrequency = 2;
    public float peanutMissileForce = 100f;
    public float flipNormalsLifeTime = 4f;
    public float circleLaserLifeTime = 4f;

    public bool animReady;

    private Color originalColor;
    private int yoshiEggCounter = 0;
    private int peanutMissileCounter = 0;
    private int maxHp;
    private int counter = 1;
    private int yoshiEggPhase = 0;
    private float yoshiEggFrequency;
    private float circleLaserLifeTimeCounter = 0;
    private float flipNormalsLifeTimeCounter = 0;
    private float peanutMissileSpawnTimer = 0;
    private float delay = 0;
    private float attackDelay = 0;
    private float pauseTimer = 0;
    private float shootTimer;
    private bool blinking;
    private bool firstPosition = true;
    private bool defaultShot = true;
    private bool triggered = false;
    private bool soundTriggered = false;
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
        audioSource = this.GetComponent<AudioSource>();

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
            mülleimer.GetComponent<Animator>().enabled = true;
            phase = 2;
        }

        if (this.hp <= thirdPhaseTrigger)
        {
            mihawkSword.GetComponent<Rigidbody>().AddForce(Random.Range(0, 10), 0, Random.Range(0, -20));
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
        }
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

    void PlaySound(int audioClip)
    {
        audioSource.PlayOneShot(audioClips[audioClip]);
        audioSource.Play();
    }

    void PhaseTwo()
    {
        ChangeColor(0);
        if (!triggered)
        {
            anim.SetBool("YoshiAttack", false);
            anim.SetTrigger("Transition");
            soundTriggered = false;
            triggered = true;
        }

        if (flipNormals) FlipNormalsAttack();
    }

    void PhaseThree()
    {
        bool soundTriggered = false;
        if (!soundTriggered)
        {
            audioSource.clip = audioClips[7];
            audioSource.Play();
            soundTriggered = true;
        }
        flipNormalInstantiate.SetActive(false);

        ChangeColor(1);
        if (peanutPhase)
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
        if (yoshiEggPhase < 3)
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

    void GenerateCracks()
    {

        switch (counter)
        {
            case 1:
                cracks[0].SetActive(true);
                counter += 1;
                break;
            case 2:
                cracks[1].SetActive(true);
                counter += 1;
                break;
            case 3:
                cracks[2].SetActive(true);
                counter += 1;
                break;
        }
    }

    void YoshiEggsAttack()
    {
        if(!soundTriggered )
        {
            audioSource.PlayOneShot(audioClips[13]);
            soundTriggered = true;
        }
        anim.SetBool("YoshiAttack1", true);
        anim.SetBool("YoshiAttack2", false);
        anim.SetBool("YoshiAttack3", false);

        if (animReady)
        {
            animReady = false;

            GameObject yoshiEggInstance = Instantiate(yoshiEggs[0], yoshiEggSpawnPosition.transform.position, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
        }

    }

    void CameraShake()
    {
        StartCoroutine(camera.GetComponent<CameraShake>().Shake(1, 0.3f));
    }

    void YoshiEggsAttackSec()
    {
        anim.SetBool("YoshiAttack1", false);
        anim.SetBool("YoshiAttack2", true);
        anim.SetBool("YoshiAttack3", false);

        if (animReady)
        {
            animReady = false;

            GameObject yoshiEggInstance = Instantiate(yoshiEggs[1], yoshiEggSpawnPosition.transform.position, Quaternion.identity);
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

            GameObject yoshiEggInstance = Instantiate(yoshiEggs[2], yoshiEggSpawnPosition.transform.position, Quaternion.identity);
            yoshiEggInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * yoshiEggSpeed);
        }

    }

    void FlipNormalsAttack()
    {
        if(!soundTriggered)
        {
            audioSource.PlayOneShot(audioClips[4]);
            soundTriggered = true;
        }
        anim.SetTrigger("FlipNormals");
        flipNormalInstantiate.SetActive(true);
    }

    void PeanutStreamAttack()
    {
        anim.SetBool("Peanut", true);
        anim.SetBool("CircleLaser", false);
        circleLaserInstantiate.SetActive(false);

        if (firstPosition && animReady)
        {
            Instantiate(peanutMissile, peanutMissileSpawnPosition[0].position, peanutMissileSpawnPosition[0].rotation);
            peanutMissile.GetComponent<Jussi_peanutMissile>().defaultTarget = !peanutMissile.GetComponent<Jussi_peanutMissile>().defaultTarget;
            firstPosition = false;
            animReady = false;
        }
        else if (!firstPosition && animReady)
        {
            Instantiate(peanutMissile, peanutMissileSpawnPosition[1].position, peanutMissileSpawnPosition[1].rotation);
            peanutMissile.GetComponent<Jussi_peanutMissile>().defaultTarget = !peanutMissile.GetComponent<Jussi_peanutMissile>().defaultTarget;
            firstPosition = true;
            animReady = false;
        }

    }

    void SwapPhase()
    {
        peanutPhase = !peanutPhase;
    }

    void LaserAttack()
    {
        anim.SetBool("CircleLaser", true);
        anim.SetBool("Peanut", false);

        if (animReady)
        {
            circleLaserInstantiate.SetActive(true);
            if (laserAttackDone)
            {
                laserAttackDone = false;
                peanutPhase = true;
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

        if (col.gameObject.CompareTag("Ultimate"))
        {
            StartCoroutine(camera.GetComponent<CameraShake>().Shake(1, 0.15f));
            audioSource.PlayOneShot(audioClips[Random.Range(8, 10)]);
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

    void ChangeColor(int texture)
    {
        for (int i = 0; i < jussiBody.Length; i++)
        {
            jussiBody[i].GetComponent<SkinnedMeshRenderer>().material.mainTexture = textures[texture];
        }
    }
}
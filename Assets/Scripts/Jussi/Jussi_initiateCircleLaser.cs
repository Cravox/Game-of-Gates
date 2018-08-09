using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_initiateCircleLaser : MonoBehaviour
{
    public float circleLaserDelay = 0.1f;
    public float shootFrequency = 0.5f;
    public float dontMoveFrequency = 2f;
    public float speed = 3;
    public int circleLaserLimit = 3;
    public GameObject circleLaser;

    private AudioSource audio;
    private int shotCounter = 0;
    private float initTimer;
    private float circleLaserDelayTimer;
    private Rigidbody rb;
    private float startSpeed;
    private float moveTimer;

    void Awake()
    {
        audio = this.GetComponent<AudioSource>();
        initTimer = shootFrequency;
        this.rb = this.gameObject.GetComponent<Rigidbody>();
        startSpeed = speed;
    }

    void Update()
    {
        this.rb.velocity = new Vector3(speed, 0, 0);
        moveTimer += Time.deltaTime;

        if(moveTimer >= dontMoveFrequency)
        {
            speed = 0;
            circleLaserDelayTimer += Time.deltaTime;

            if(circleLaserDelayTimer >= circleLaserDelay && shotCounter <= circleLaserLimit)
            {
                audio.Play();
                Instantiate(circleLaser, this.transform.position, Quaternion.identity);
                shotCounter += 1;
                circleLaserDelayTimer -= circleLaserDelay;
            }

            if (shotCounter >= circleLaserLimit)
            {
                speed = startSpeed;
                moveTimer = 0;
                shotCounter = 0;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        this.speed = speed * -1;
    }
}

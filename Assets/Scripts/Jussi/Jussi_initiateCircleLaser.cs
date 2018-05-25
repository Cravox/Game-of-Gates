using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_initiateCircleLaser : MonoBehaviour
{
    public float circleLaserDelay = 0.1f;
    public float shootFrequency = 0.5f;
    public GameObject circleLaser;

    private int shotCounter;
    private float initTimer;
    private float circleLaserDelayTimer;
    private Rigidbody rb;
    private float speed;
    private float moveTimer;
    private bool dontMove = false;


    void Awake()
    {
        initTimer = shootFrequency;
        speed = 3;
        this.rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        this.rb.velocity = new Vector3(speed, 0, 0);
        initTimer += Time.deltaTime;

        if (initTimer > shootFrequency)
        {
            Instantiate(circleLaser, this.transform.position, this.transform.rotation);
            initTimer -= shootFrequency;

        }

    }

    void OnCollisionEnter(Collision col)
    {
        this.speed = speed * -1;
    }
}

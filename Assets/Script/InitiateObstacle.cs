using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateObstacle : MonoBehaviour
{
    private float initTimer;
    public float shootFrequence;
    private Rigidbody rb;

    private float speed;

    void Awake()
    {
        initTimer = shootFrequence;
        speed = 3;
        this.rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        this.rb.velocity = new Vector3(speed, 0, 0);

        initTimer -= Time.deltaTime;
        
        if(initTimer < 0)
        {
            GameObject obstacleRessource = (GameObject)Resources.Load("Obstacle_1");
            GameObject obstacleInstance = Object.Instantiate(obstacleRessource, this.transform.position, this.transform.rotation);

            initTimer = shootFrequence;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        this.speed = speed*-1;
    }
}
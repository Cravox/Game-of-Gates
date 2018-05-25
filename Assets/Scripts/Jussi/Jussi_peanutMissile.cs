using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Jussi_peanutMissile : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;
    public float rotateSpeed = 25f;
    public float activationTime = 0.2f;
    public int hp = 5;

    public bool defaultTarget = true;
    private float activationTimer = 0;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(target == null)
        {
            defaultTarget = !defaultTarget;
        }

        if(defaultTarget)
        {
            target = GameObject.Find("hitTarget_0").GetComponent<Transform>();
        }else
        {
            target = GameObject.Find("hitTarget_1").GetComponent<Transform>();
        }


        activationTimer += Time.deltaTime;
        if (this.hp <= 0) Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (activationTimer >= activationTime)
        {
            Vector3 direction = (Vector3)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = new Vector3(0, 0, -rotateAmount * rotateSpeed);
            rb.velocity = transform.up * speed;
        }
    }

    void OnTriggerEnter(Collider col)

    {
        if(col.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().hp -= 1;
            Destroy(this.gameObject);
        }

        if(col.gameObject.CompareTag("Bullet"))
        {
            this.hp -= 1;
        }
    }

}

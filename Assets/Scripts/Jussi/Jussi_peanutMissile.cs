using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Jussi_peanutMissile : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float rotateSpeed = 50f;
    public int hp = 5;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 direction = (Vector3)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = new Vector3(0, 0, -rotateAmount * rotateSpeed);
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter(Collider col)

    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_circleLaser : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;

    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        target = GameObject.Find("hitTarget_0").GetComponent<Transform>();
        Vector3 direction = (Vector3)target.position - rb.position;
        direction.Normalize();
        rb.velocity = direction * speed;
        Destroy(this.gameObject, 10f);
    }

    void Update()
    {
        if(target == null)
        {
            target = GameObject.Find("hitTarget_1").GetComponent<Transform>();
        }
    }

    void OnTriggerEnter(Collider col)

    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().hp -= 1;
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_circleLaser : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;

    private Rigidbody rb;

    void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            target = GameObject.Find("hitTarget_"+i).GetComponent<Transform>();
        }

        //if(GameObject.Find("hitTarget_0") != null)
        //{
        //}else
        //{
        //    target = GameObject.Find("hitTarget_1").GetComponent<Transform>();
        //}

        rb = this.GetComponent<Rigidbody>();
        Vector3 direction = (Vector3)target.position - rb.position;
        direction.Normalize();

        rb.velocity = direction * speed;
        Destroy(this.gameObject, 3f);
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
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

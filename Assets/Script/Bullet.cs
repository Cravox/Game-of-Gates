using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(this.gameObject, 4);
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().hp -= 2;
            Destroy(this.gameObject);
        }
    }
}

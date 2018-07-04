using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_yoshiEgg : MonoBehaviour
{
    private Rigidbody rb;

    private int enemyDamage = 1;


    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(this.gameObject, 5);
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().hp -= enemyDamage;
            Destroy(this.gameObject);
        }
    }
}

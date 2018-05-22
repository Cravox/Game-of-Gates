using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float bulletTimer;
    private int enemyDamage = 1;


    void Awake()
    {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        bulletTimer = 0.3f;
    }

    void Start()
    {
        Destroy(this.gameObject, 5);
    }

    void Update()
    {
        bulletTimer -= Time.deltaTime;

        if (bulletTimer < 0)
        {
            anim.SetTrigger("Timer");
        }
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

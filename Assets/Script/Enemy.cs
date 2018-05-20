using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;

    public float shootFrequence;
    private float moveY;
    private float shotCounter;

    public Transform shotSpawnPosition;
    private Animator anim;
    public GameObject bullet;

    void Awake()
    {
        shotCounter = shootFrequence;
    }

    void Update()
    {
        shotCounter -= Time.deltaTime;

        if (this.hp <= 0)
        {
            Destroy(this.gameObject, 0.2f);
        }

        if(shotCounter < 0)
        {
            shotCounter = shootFrequence;
        }
    }

    void OnTriggerEnter(Collider col)
    {

    }

    void Shoot()
    {
        Vector3 spawn = shotSpawnPosition.transform.position;
        GameObject bulletInstance = Object.Instantiate(bullet, spawn, transform.rotation);
    }

    void Death()
    {
        if(this.hp <= 0)
        {
            Destroy(this.gameObject, 0.1f);
        }
    }
}

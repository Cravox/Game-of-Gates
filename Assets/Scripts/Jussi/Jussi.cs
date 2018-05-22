using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi : MonoBehaviour
{
    public int hp;

    public float shootFrequence;
    private float moveY;
    private float shootTimer;
    private float shootFrequency;

    public Transform shotSpawnPosition;
    private Animator anim;
    public GameObject bullet;

    void Awake()
    {

    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (this.hp <= 0)
        {
            Destroy(this.gameObject, 0.2f);
        }

        if(shootTimer >= shootFrequence)
        {
            Shoot();
            shootTimer -= shootFrequence;
        }
    }

    void OnTriggerEnter(Collider col)
    {

    }

    void Shoot()
    {
        Vector3 spawn = shotSpawnPosition.transform.position;
        GameObject bulletInstance = Object.Instantiate(bullet, spawn, transform.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(new Vector3(-1,0,0)*250);
    }
}

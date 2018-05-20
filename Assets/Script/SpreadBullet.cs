using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{

    public float range;
    public int damage;

    void Start()
    {
        Destroy(this.gameObject, range);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy")) col.gameObject.GetComponent<Enemy>().hp -= damage;
    }
}

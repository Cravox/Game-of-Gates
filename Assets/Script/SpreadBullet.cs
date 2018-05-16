using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{

    void Start()
    {
        Destroy(this.gameObject, 0.3f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy")) col.gameObject.GetComponent<Enemy>().hp -= 2;
    }
}

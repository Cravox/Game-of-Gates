using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_blackHole : MonoBehaviour
{
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().hp -= 1;
        }
    }
}

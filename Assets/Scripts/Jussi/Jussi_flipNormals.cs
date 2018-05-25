using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_flipNormals : MonoBehaviour
{
    public GameObject blackHole;
    private Rigidbody rb;
    
    void Awake()
    {
        this.rb = this.gameObject.GetComponent<Rigidbody>();
        Destroy(this.gameObject, 5);
    }
    void Update()
    {
        this.rb.velocity = new Vector3(0, -5, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().hp -= 1;
            Destroy(this.gameObject);
        }

        if(col.gameObject.CompareTag("Ground"))
        {
            Instantiate(blackHole, new Vector3(this.transform.position.x, -0.574f, 0), Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        
    }
}

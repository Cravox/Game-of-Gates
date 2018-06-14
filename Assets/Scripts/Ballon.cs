using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Ballon : MonoBehaviour
{
    public int assignedScene;
    public float ballonForce = 0.5f;
    private Rigidbody rb;

    private int hp = 15;
    private bool hoverUp = true;
    private float ballonHover = 0.75f;
    private float ballonHoverTimer = 0;

    void Start()
    {
        rb = this.GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        if(hp <= 0)
        {
            SceneManager.LoadScene("LoadingScreen");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Bullet"))
        {
            this.hp -= 3;
        }
    }
}

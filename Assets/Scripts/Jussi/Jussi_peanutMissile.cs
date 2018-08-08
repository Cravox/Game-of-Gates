using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Jussi_peanutMissile : MonoBehaviour
{
    public MeshRenderer renderer;
    public Transform target;
    public float speed = 1f;
    public float rotateSpeed = 25f;
    public float activationTime = 0.2f;
    public int hp = 5;
    public bool defaultTarget = true;
    public float lifeTime = 10f;

    private Color originalColor;
    private float activationTimer = 0;
    private Rigidbody rb;
    
    void Start()
    {
        if (GameObject.Find("hitTarget_0") != null)
        {
            target = GameObject.Find("hitTarget_0").GetComponent<Transform>();
        }
        else
        {
            target = GameObject.Find("hitTarget_1").GetComponent<Transform>();
        }


        Destroy(this.gameObject, lifeTime);
        originalColor = renderer.materials[0].color;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        activationTimer += Time.deltaTime;
        if (this.hp <= 0) Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (activationTimer >= activationTime)
        {
            Vector3 direction = (Vector3)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = new Vector3(0, 0, -rotateAmount * rotateSpeed);
            rb.velocity = transform.up * speed;
        }
    }

    void OnTriggerEnter(Collider col)

    {
        if(col.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().hp -= 1;
            Destroy(this.gameObject);
        }

        if(col.gameObject.CompareTag("Bullet"))
        {
            renderer.materials[0].color = Color.white;
            Invoke("ResetColor", 0.05f);
            this.hp -= 1;
        }
    }

    void ResetColor()
    {
        renderer.materials[0].color = originalColor;
    }

}

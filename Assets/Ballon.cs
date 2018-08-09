using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour {

    public int hp = 20;
    private MeshRenderer renderer;
    private Color ogcolor;

	// Use this for initialization
	void Start () {
        renderer = this.GetComponent<MeshRenderer>();
        ogcolor = this.GetComponent<MeshRenderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0) Destroy(this.gameObject);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Bullet"))
        {
            this.hp -= 2;
            renderer.material.color = Color.white;
            Invoke("ResetColor", 0.075f);
            Destroy(col.gameObject);
        }
    }

    void ResetColor()
    {
        renderer.material.color = ogcolor;
    }
}

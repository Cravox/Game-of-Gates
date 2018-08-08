using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jussi_flipNormalCloud : MonoBehaviour {

    public GameObject flipNormal;
    public float instantiateTime = 0.1f;
    private float instantiateTimer;
    private bool shot = false;


	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
        instantiateTimer += Time.deltaTime;

        if(instantiateTimer >= instantiateTime && !shot)
        {
            shot = true;
            Instantiate(flipNormal, this.transform.position, this.transform.rotation);
        }
	}
}

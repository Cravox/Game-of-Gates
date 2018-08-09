using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ghost : MonoBehaviour {

    public bool tutorial = false;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!tutorial) transform.position = new Vector3(transform.position.x, transform.position.y + 1*Time.deltaTime, transform.position.z);
	}
}

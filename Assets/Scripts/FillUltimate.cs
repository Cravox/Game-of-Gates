using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillUltimate : MonoBehaviour {

    public Image ultishit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col)
    {
        ultishit.fillAmount = 1;
        //col.gameObject.GetComponent<Player_weapon>().ultiMeter.fillAmount = 1;
    }
}

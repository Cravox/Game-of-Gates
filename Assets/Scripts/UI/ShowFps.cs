using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFps : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        GetComponent<Text>().text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString() + " FPS";
    }
}

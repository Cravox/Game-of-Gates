using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    AsyncOperation operation;

    public string scene;
    public float timer = 0;
    private float progress;
    private bool sceneLoaded;

    void Update()
    {
        timer += Time.deltaTime;
        //string loadedScene = "Jussi_Multiplayer";
        if(timer >= 2 && !sceneLoaded)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            sceneLoaded = true;
        }
    }
}

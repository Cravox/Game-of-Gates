using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Slider progressBar;
    AsyncOperation operation;

    private float progress;
    private bool sceneLoaded = false;

    void Update()
    {
        //StartCoroutine("LoadAsynchronously");
        string loadedScene = "1";
        progress = progressBar.value + Random.Range(0.002f, 0.01f);

        progressBar.value = progress;

        if (progressBar.value == 1f && !sceneLoaded)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(loadedScene);
            sceneLoaded = true;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{


    void Update()
    {
        StartCoroutine("LoadAsynchronously");
    }

    public void LoadScene(int sceneIndex)
    {

    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return new WaitForSeconds(2f);
        }
    }

}

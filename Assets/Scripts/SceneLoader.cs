using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public int assignedScene;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerStay(Collider c)
    {
        if (Input.GetButtonDown("Select"))
            LoadScene(assignedScene);

    }

    public void LoadScene(int sceneSelected)
    {
        SceneManager.LoadScene(sceneSelected.ToString());
    }

    public void LoadScene(string sceneSelected)
    {
        SceneManager.LoadScene(sceneSelected);
    }

}

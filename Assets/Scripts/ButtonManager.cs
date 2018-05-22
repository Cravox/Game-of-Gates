using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void OnClickLevelSelect()
    {
        SceneManager.LoadScene("levelSelect");
    }

    public void OnClickOptions()
    {
        //SceneManager.LoadScene("Options");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

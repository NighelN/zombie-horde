using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour
{
    public void LoadScene(string sceneName = "MainMenu")
    {
        SceneManager.LoadScene(sceneName);
    }
}

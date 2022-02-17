using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void load(string sceneName)
    {
        print("Loading " + sceneName + "...");
        SceneManager.LoadSceneAsync(sceneName);
    }
}

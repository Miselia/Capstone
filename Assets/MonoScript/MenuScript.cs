using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // Application.LoadLevel is apparently outdated, wants to use SceneManager instead
        Application.LoadLevel(sceneName);
        //SceneManager.LoadScene(sceneName);
    }
}
    

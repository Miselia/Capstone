using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;
using Assets.Systems;
//using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;
using Assets.Entities;
//using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void Awake()
    {
        World.Active.GetExistingSystem<DrawSystem>().Enabled = false;
        World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = false;
        World.Active.GetExistingSystem<ControlSystem>().Enabled = false;
        World.Active.GetExistingSystem<DeletionSystem>().Enabled = false;
        World.Active.GetExistingSystem<MovementSystem>().Enabled = false;
        World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = false;
        World.Active.GetExistingSystem<TestingSystem>().Enabled = false;

    }
}
    

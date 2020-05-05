using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;
using Assets.Systems;
//using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private List<GameObject> titles;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void Awake()
    {
        int rand = Random.Range(0, 4);
        characters[rand].SetActive(true);
        titles[rand].SetActive(true);
        World.Active.GetExistingSystem<DrawSystem>().Enabled = false;
        World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = false;
        World.Active.GetExistingSystem<ControlSystem>().Enabled = false;
        World.Active.GetExistingSystem<DeletionSystem>().Enabled = false;
        World.Active.GetExistingSystem<MovementSystem>().Enabled = false;
        World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = false;
        World.Active.GetExistingSystem<TestingSystem>().Enabled = false;
        World.Active.GetExistingSystem<BuffSystem>().Enabled = false;
        World.Active.GetExistingSystem<GravitySystem>().Enabled = false;
        World.Active.GetExistingSystem<SpawnDelaySystem>().Enabled = false;
        World.Active.GetExistingSystem<RotationSystem>().Enabled = false;

        World.Active.GetExistingSystem<QuadrantSystem>().Enabled = false;
        //World.Active.GetExistingSystem<QuadTreeJobDrawingSystem>().Enabled = false;
        //World.Active.GetExistingSystem<QuadTreeJobSystem>().Enabled = false;
        World.Active.GetExistingSystem<QuadTreeSystem>().Enabled = false;

        World.Active.GetExistingSystem<QuadTreeDrawingSystem>().Enabled = false;
        World.Active.GetExistingSystem<CollisionBoxDrawingSystem>().Enabled = false;
        //World.Active.GetExistingSystem<CollisionBoxDrawingSystem>().Enabled = false;
    }
}
    

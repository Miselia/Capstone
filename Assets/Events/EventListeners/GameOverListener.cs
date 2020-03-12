using Assets.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Entities;

public class GameOverListener : MonoBehaviour, IGenericEventListener
{
    private void Start()
    {
        EventManager.instance.RegisterListener<GameOverEvent>(this);
    }
    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is GameOverEvent)
        {
            
            GameOverEvent ge = evt as GameOverEvent;
            Debug.Log("Game Over Event UwU");
            World.Active.GetExistingSystem<DrawSystem>().Enabled = false;
            World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = false;
            World.Active.GetExistingSystem<ControlSystem>().Enabled = false;
            World.Active.GetExistingSystem<DeletionSystem>().Enabled = false;
            World.Active.GetExistingSystem<MovementSystem>().Enabled = false;
            World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = false;
            foreach (Entity e in World.Active.EntityManager.GetAllEntities())
            {
                World.Active.EntityManager.DestroyEntity(e);
            }
            if (ge.pID == 1) SceneManager.LoadScene("GameOver"); ;
            if (ge.pID == 2) SceneManager.LoadScene("GameOver"); ;
            

                return true;
        }
        return false;
    }
}

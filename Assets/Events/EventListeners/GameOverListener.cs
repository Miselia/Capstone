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
            if (ge.pID == 1) SceneManager.LoadScene("GameOver"); ;
            if (ge.pID == 2) SceneManager.LoadScene("GameOver"); ;
            foreach (Entity e in World.Active.EntityManager.GetAllEntities())
            {
                World.Active.EntityManager.AddComponent(e, typeof(DeleteComp));
            }

                return true;
        }
        return false;
    }
}

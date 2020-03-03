using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.SceneManagement;

public class DeletionSystem : ComponentSystem
{
    private Game game;
    protected override void OnStartRunning()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene") || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
        } 
    }
    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene") || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
            game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        
        Entities.ForEach((Entity e, ref DeleteComp d, ref Translation t) =>
            {
                
                    World.Active.EntityManager.DestroyEntity(e);
            });
        }
    }
}

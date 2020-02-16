using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class DeletionSystem : ComponentSystem
{
    private Game game;
    protected override void OnStartRunning()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene") || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
            game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        }
    }
    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene") || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
            Entities.ForEach((Entity e, ref DeleteComp d) =>
            {
                World.Active.EntityManager.DestroyEntity(e);
            });
        }
    }
}

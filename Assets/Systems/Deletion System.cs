using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DeletionSystem : ComponentSystem
{
    private Game game;
    protected override void OnStartRunning()
    {
        game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
    }
    protected override void OnUpdate()
    {
        
        Entities.ForEach((Entity e,ref DeleteComp d) =>
        {
            World.Active.EntityManager.DestroyEntity(e);
        });
    }
}

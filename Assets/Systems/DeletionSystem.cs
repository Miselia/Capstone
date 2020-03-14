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
        game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
    }
    protected override void OnUpdate()
    {
        
        //game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        
        Entities.ForEach((Entity e, ref DeleteComp d, ref Translation t) =>
            {
                if(d.buffer <= 0)
                    World.Active.EntityManager.DestroyEntity(e);
                d.buffer--;
            });

        
    }
}

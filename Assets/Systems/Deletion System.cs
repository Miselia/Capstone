using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DeletionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        
        Entities.ForEach((Entity e,ref DeleteComp d) =>
        {
            if (d.buffer == 0) World.Active.EntityManager.DestroyEntity(e);
            else d.buffer--;
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class SpawnDelaySystem : ComponentSystem
{
    protected override void OnUpdate()
    {

        Entities.ForEach((Entity e, ref SpawnDelayComp d) =>
        {
            if (d.timer <= 0)
            {
                World.Active.EntityManager.AddComponent(e, typeof(MovementComponent));
                World.Active.EntityManager.SetComponentData(e, new MovementComponent(d.movementVector));
                World.Active.EntityManager.AddComponent(e, typeof(CollisionComponent));
                World.Active.EntityManager.SetComponentData(e, new CollisionComponent(d.radius,d.radius));
                World.Active.EntityManager.SetComponentData(e, new Scale { Value = d.radius * 2.35f });
                World.Active.EntityManager.RemoveComponent<SpawnDelayComp>(e);
            }
            else World.Active.EntityManager.SetComponentData(e, new Scale { Value = (d.radius * 2.35f)/d.timer });
            d.timer--;
            
        });


    }
}


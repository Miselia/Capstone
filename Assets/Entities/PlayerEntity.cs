using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public static class PlayerEntity 
{
   public static Entity Create(EntityManager em, Vector2 position)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent( entity, typeof(XformComponent) );
        em.AddComponent( entity, typeof(MovementComponent) );
        em.AddComponent( entity, typeof(PlayerComponent) );
        //em.AddComponent( entity, typeof(SpriteComponent) );

        em.SetComponentData(entity, new MovementComponent { movementVector = new Vector2(0, 10) });
        em.SetComponentData(entity, new XformComponent { hasXform = true });
        em.SetComponentData(entity, new PlayerComponent { healthRemaining = 3, playerName = "Left_Player" });
        //em.SetComponentData(entity, new SpriteComponent { insert_stuff_here });

        return entity;
    }
}

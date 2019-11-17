using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public static class PlayerEntity 
{
   public static Entity Create(EntityManager em, Vector2 movementVector, Vector2 position, int playerID, int maxHealth)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent( entity, typeof(XformComponent) );
        em.AddComponent( entity, typeof(MovementComponent) );
        em.AddComponent( entity, typeof(PlayerComponent) );
        //em.AddComponent( entity, typeof(SpriteComponent) );

        em.SetComponentData(entity, new MovementComponent(movementVector));
        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new PlayerComponent(playerID, maxHealth));
        //em.SetComponentData(entity, new SpriteComponent { insert_stuff_here });

        return entity;
    }
}

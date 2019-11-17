using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public static class PlayerEntity 
{
   public static Entity Create(EntityManager em, Vector2 movementVector, Vector2 position, int playerID, int maxHealth, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent( entity, typeof(XformComponent) );
        em.AddComponent( entity, typeof(MovementComponent) );
        em.AddComponent( entity, typeof(PlayerComponent) );
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Translation));
        
        em.SetComponentData(entity, new MovementComponent(movementVector));
        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new PlayerComponent(playerID, maxHealth));
        em.SetSharedComponentData(entity, new RenderMesh {mesh = mesh, material = mat});

        return entity;
    }
}

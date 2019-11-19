using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public static class PlayerEntity 
{

   public static Entity Create(EntityManager em, Vector2 position, Vector2 movementVector, int playerID, int maxHealth, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();

        
        em.AddComponent( entity, typeof(MovementComponent) );
        em.AddComponent( entity, typeof(PlayerComponent) );
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Translation));
        
        em.SetComponentData(entity, new MovementComponent(movementVector));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        em.SetComponentData(entity, new CollisionComponent(radius));
        em.SetComponentData(entity, new PlayerComponent(playerID, maxHealth));
        em.SetSharedComponentData(entity, new RenderMesh {mesh = mesh, material = mat});

        return entity;
    }
}

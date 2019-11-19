using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public static class PlayerEntity 
{
<<<<<<< HEAD
   public static Entity Create(EntityManager em, Vector2 position, Vector2 movementVector, int playerID, int maxHealth, Mesh mesh, Material mat)
=======
   public static Entity Create(EntityManager em, Vector2 movementVector, Vector2 position, float radius, int playerID, int maxHealth, Mesh mesh, Material mat)
>>>>>>> bf301571769521d317667a3ffc3825c0b294e06e
    {
        Entity entity = em.CreateEntity();

        
        em.AddComponent( entity, typeof(MovementComponent) );
        em.AddComponent( entity, typeof(PlayerComponent) );
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Translation));
        
        em.SetComponentData(entity, new MovementComponent(movementVector));
<<<<<<< HEAD
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
=======
        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(radius));
>>>>>>> bf301571769521d317667a3ffc3825c0b294e06e
        em.SetComponentData(entity, new PlayerComponent(playerID, maxHealth));
        em.SetSharedComponentData(entity, new RenderMesh {mesh = mesh, material = mat});

        return entity;
    }
}

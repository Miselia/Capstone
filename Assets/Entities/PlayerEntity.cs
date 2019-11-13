using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public static class PlayerEntity 
{
    
   
   public static Entity Create(EntityManager em, Vector2 position)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent( entity, typeof(XformComponent) );
        em.AddComponent( entity, typeof(MovementComponent) );
        em.AddComponent( entity, typeof(PlayerComponent) );
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(PlayerComponent));
        

        em.SetComponentData(entity, new MovementComponent { movementVector = new Vector2(0, 10) });
        em.SetComponentData(entity, new XformComponent { hasXform = true });
        em.SetComponentData(entity, new PlayerComponent { playerID = 1, healthRemaining = 3 });
        em.SetSharedComponentData(entity, new RenderMesh {mesh = new Mesh(), material = new Material(Shader.Find("Specular"))});

        return entity;
    }
}

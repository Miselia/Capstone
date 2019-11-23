using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public static class BoundaryEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 normal)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(BoundaryComponent));
        em.AddComponent(entity, typeof(Translation));

        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(0.0f, 1000));
        em.SetComponentData(entity, new BoundaryComponent(normal));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });

        return entity;
    }
}

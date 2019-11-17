using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public static class BoundaryEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 normal)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(BoundaryComponent));
        //em.AddComponent(entity, typeof(SpriteComponent));

        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(0.0f));
        em.SetComponentData(entity, new BoundaryComponent(normal));
        //em.SetComponentData(entity, new SpriteComponent("sprite_file");

        return entity;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public static class ProjectileEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 movementVector, float radius)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(MovementComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        //em.AddComponent(entity, typeof(SpriteComponent));

        em.SetComponentData(entity, new MovementComponent(movementVector));
        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(radius));
        //em.SetComponentData(entity, new SpriteComponent { Sprite = });

        return entity;
    }
}
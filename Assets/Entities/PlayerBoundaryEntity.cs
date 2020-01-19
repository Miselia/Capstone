﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Assets.Resources;

public static class PlayerBoundaryEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 normal, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(PlayerBoundaryComponent));
        em.AddComponent(entity, typeof(Translation));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Scale));

        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(0f, 1000));
        em.SetComponentData(entity, new PlayerBoundaryComponent(normal));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });
        em.SetComponentData(entity, new Scale { Value = Constants.PlayerBoundarySize });

        return entity;
    }
}

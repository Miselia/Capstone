using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using Assets.Resources;

public static class ProjectileEntity
{
    public static Entity Create(EntityManager em, int damage, Vector2 position, Vector2 movementVector, float radius, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(MovementComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(ProjectileComponent));
        em.AddComponent(entity, typeof(Translation));
        em.AddComponent(entity, typeof(Rotation));
        em.AddComponent(entity, typeof(Scale));

        em.SetComponentData(entity, new Scale { Value = radius * 2.35f});
        em.SetComponentData(entity, new ProjectileComponent(0,damage));
        em.SetComponentData(entity, new MovementComponent(movementVector));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        em.SetComponentData(entity, new Rotation { Value = quaternion.Euler(0,0, Mathf.Atan2(movementVector.x, movementVector.y))});
        em.SetComponentData(entity, new CollisionComponent(radius, radius));
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });

        return entity;
    }
}

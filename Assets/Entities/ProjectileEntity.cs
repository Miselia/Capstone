using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public static class ProjectileEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(MovementComponent));
        em.AddComponent(entity, typeof(RotationComponent));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(ProjectileComponent));
        em.AddComponent(entity, typeof(Translation));



        em.SetComponentData(entity, new MovementComponent { movementVector = new Vector2(0,5) });
        em.SetComponentData(entity, new XformComponent { hasXform = true });
        em.SetComponentData(entity, new RotationComponent { rotation = new Quaternion(0, 1, 0, 0) });
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });


        return entity;
    }
}

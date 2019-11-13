using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public static class ProjectileEntity
{
    public static Entity Create(EntityManager em, Vector2 position)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(MovementComponent));
        em.AddComponent(entity, typeof(RotationComponent));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));


        em.SetComponentData(entity, new MovementComponent { movementVector = new Vector2(0, -2) });
        em.SetComponentData(entity, new XformComponent { hasXform = true });
        em.SetComponentData(entity, new RotationComponent { rotation = new Quaternion(0, 1, 0, 0) });
        em.SetSharedComponentData(entity, new RenderMesh { mesh = new Mesh(), material = new Material(Shader.Find("Specular")) });


        return entity;
    }
}

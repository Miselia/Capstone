using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public static class ProjectileEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 movementVector, float radius, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(MovementComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(ProjectileComponent));
        em.AddComponent(entity, typeof(Translation));

        em.SetComponentData(entity, new MovementComponent(movementVector));
        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(radius));
        //em.SetComponentData(entity, new RotationComponent { rotation = new Quaternion(0, 1, 0, 0) });
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });

>>>>>>> master

        return entity;
    }
}

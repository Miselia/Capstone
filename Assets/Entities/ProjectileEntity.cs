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
    public static Entity Create(EntityManager em, int damage, Vector2 position, Vector2 movementVector, float radius, int timer, Mesh mesh, Material mat, byte mask = 0x03, bool rotateWithDirection = true, Vector2 initialRotation = new Vector2(), float extraScale = 1)
    {
        Entity entity = em.CreateEntity();

        //em.AddComponent(entity, typeof(MovementComponent));
        //em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(SpawnDelayComp));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(ProjectileComponent));
        em.AddComponent(entity, typeof(Translation));
        em.AddComponent(entity, typeof(Rotation));
        em.AddComponent(entity, typeof(Scale));
        em.AddComponent(entity, typeof(QuadTreeReferenceComponent));
        em.AddComponent(entity, typeof(RotationComponent));

        //em.SetComponentData(entity, new Scale { Value = radius*extraScale/2});
        em.SetComponentData(entity, new ProjectileComponent(0,damage));
        em.SetComponentData(entity, new SpawnDelayComp(movementVector, timer, radius, mask, extraScale));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        //em.SetComponentData(entity, new Rotation { Value = quaternion.Euler(0,0, Mathf.Atan2(movementVector.x, movementVector.y))});
        //em.SetComponentData(entity, new CollisionComponent(radius, radius, 0x03));
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });
        em.SetComponentData(entity, new QuadTreeReferenceComponent(-1));
        if (initialRotation == new Vector2())
        {
            em.SetComponentData(entity, new RotationComponent(rotateWithDirection));
            em.SetComponentData(entity, new Rotation { Value = quaternion.Euler(0, 0, Mathf.Atan2(movementVector.x, movementVector.y)) });
        }
        else
        {
            em.RemoveComponent(entity, typeof(RotationComponent));
            em.SetComponentData(entity, new Rotation { Value = quaternion.RotateZ(Mathf.Atan2(initialRotation.x, initialRotation.y)) });
        }

        return entity;
    }
}

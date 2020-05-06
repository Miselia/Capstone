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
    public static Entity Create(EntityManager em, int damage, Vector2 position, Vector2 movementVector, float radius, float timer, Mesh mesh, Material mat, byte mask = 0x03, bool rotateWithDirection = true, Vector2 initialRotation = new Vector2(), float extraScale = 1)
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
        em.AddComponent(entity, typeof(AffectedByGravityComponent));

        //em.SetComponentData(entity, new Scale { Value = radius*extraScale/2});
        em.SetComponentData(entity, new ProjectileComponent(0,damage));
        em.SetComponentData(entity, new SpawnDelayComp(movementVector, timer, radius, mask, extraScale));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        //em.SetComponentData(entity, new Rotation { Value = quaternion.Euler(0,0, Mathf.Atan2(movementVector.x, movementVector.y))});
        //em.SetComponentData(entity, new CollisionComponent(radius, radius, 0x03));
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });
        em.SetComponentData(entity, new QuadTreeReferenceComponent(-1));
        em.SetComponentData(entity, new RotationComponent(rotateWithDirection));

        if (rotateWithDirection == true)
        {
            // If you want a projectile to move with the direction (If it changes direction, it faces the way it moves) then this needs to be true
            // Funnily enough, projectiles that are set to rotate with direction and have a direction of (0,0) will stay in place and never rotate
            em.SetComponentData(entity, new Rotation { Value = quaternion.Euler(0, 0, Mathf.Atan2(-movementVector.x, movementVector.y)) });
        }
        else
        {
            // If a projectile moves AND rotates over time, then set this to false 
            // If a projectile needs an initial rotation of something OTHER than (0,0) AND doesn't rotate, then set to false and in Spawner
            // remove the RotationComponent
            // If a projectile spawns and doesn't move, nor is affected by gravity, then initial rotation should be (0,0) and rotateWithDireciton = false
            //      then, the RotationComponent and AffectedByGravityComponent should be removed in the spawner
            em.SetComponentData(entity, new Rotation { Value = quaternion.RotateZ(Mathf.Atan2(initialRotation.x, initialRotation.y)) });
        }

        return entity;
    }
}

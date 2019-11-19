using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CollisionDetectionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Dictionary<Entity, List<Entity>> checkedPairs = new Dictionary<Entity, List<Entity>>();
        bool skipFlag = false;

        Entities.ForEach((Entity firstEntity, ref Translation xform, ref CollisionComponent collComp) =>
        {
            if(!checkedPairs.ContainsKey(firstEntity))
            {
                checkedPairs.Add(firstEntity, new List<Entity>());
            }

            Entities.ForEach((Entity secondEntity, ref Translation xform, ref CollisionComponent collComp) =>
            {
                if(firstEntity == secondEntity)
                {
                    skipFlag = true;
                }
                if(checkedPairs[firstEntity].Contains(secondEntity))
                {
                    skipFlag = true;
                }
                if(EntityManager.HasComponent<BoundaryComponent>(firstEntity) && EntityManager.HasComponent<BoundaryComponent>(secondEntity))
                {
                    skipFlag = true;
                }

                if (!skipFlag)
                {
                    // These internal method calls should instead be exported to a Event/Listener system to handle collision calculations
                    if (!EntityManager.HasComponent<BoundaryComponent>(firstEntity) && EntityManager.HasComponent<BoundaryComponent>(secondEntity))
                    {
                        HandleCircleCollisionWithBoundary(firstEntity, secondEntity);
                    }
                    if (EntityManager.HasComponent<BoundaryComponent>(firstEntity) && !EntityManager.HasComponent<BoundaryComponent>(secondEntity))
                    {
                        HandleCircleCollisionWithBoundary(secondEntity, firstEntity);
                    }
                    if (!EntityManager.HasComponent<ProjectileComponent>(firstEntity) && EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                    {
                        HandlePlayerCollisionWithProjectile(firstEntity, secondEntity);
                    }
                    if (EntityManager.HasComponent<ProjectileComponent>(firstEntity) && !EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                    {
                        HandlePlayerCollisionWithProjectile(secondEntity, firstEntity);
                    }
                }
                skipFlag = false;
            });
        });
    }

    private void HandleCircleCollisionWithBoundary(Entity circleEntity, Entity boundaryEntity)
    {
        Vector3 circleVector = EntityManager.GetComponentData<Translation>(circleEntity).Value;
        Vector3 boundaryVector = EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = EntityManager.GetComponentData<CollisionComponent>(circleEntity).collisionRadius;

        if(EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(circleVector.x, boundaryVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                Console.WriteLine("circle entity collide with boundary");
            }
        }
        if(EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                Console.WriteLine("circle entity collide with boundary");
            }
        }
    }

    private void HandlePlayerCollisionWithProjectile(Entity playerEntity, Entity ProjectileEntity)
    {
        Vector3 playerVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 projectileVector = EntityManager.GetComponentData<Translation>(ProjectileEntity).Value;
        float firstRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;
        float secondRadius = EntityManager.GetComponentData<CollisionComponent>(ProjectileEntity).collisionRadius;

        if( (new Vector2(playerVector.x,playerVector.y) - new Vector2(projectileVector.x,projectileVector.y)).magnitude > (firstRadius + secondRadius))
        {
            Console.WriteLine("player entity collide with projectile");
        }
    }
}

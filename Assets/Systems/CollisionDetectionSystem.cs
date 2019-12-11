using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CollisionDetectionSystem : ComponentSystem
{
    Dictionary<Entity, List<Entity>> collidingPairs = new Dictionary<Entity, List<Entity>>();
    protected override void OnUpdate()
    {
        Dictionary<Entity, List<Entity>> checkedPairs = new Dictionary<Entity, List<Entity>>();
        bool skipFlag = false;

        Entities.ForEach((Entity firstEntity, ref Translation xform, ref CollisionComponent collComp) =>
        {
            if(!collidingPairs.ContainsKey(firstEntity))
            {
                collidingPairs.Add(firstEntity, new List<Entity>());
            }
            if(!checkedPairs.ContainsKey(firstEntity))
            {
                checkedPairs.Add(firstEntity, new List<Entity>());
            }

            Entities.ForEach((Entity secondEntity, ref Translation transform, ref CollisionComponent collisionComp) =>
            {
                if(!collidingPairs.ContainsKey(secondEntity))
                {
                    collidingPairs.Add(secondEntity, new List<Entity>());
                }
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
                if (collidingPairs[firstEntity].Contains(secondEntity) || collidingPairs[secondEntity].Contains(firstEntity))
                {
                    skipFlag = true;
                }

                if (!skipFlag)
                {
                    // These internal method calls should instead be exported to a Event/Listener system to handle collision calculations
                    if (!EntityManager.HasComponent<BoundaryComponent>(firstEntity) && EntityManager.HasComponent<BoundaryComponent>(secondEntity))
                    {
                        HandleCircleCollisionWithBoundary(firstEntity, secondEntity);
                        //Debug.Log("Circle and Wall Collision Check");
                    }
                    if (EntityManager.HasComponent<BoundaryComponent>(firstEntity) && !EntityManager.HasComponent<BoundaryComponent>(secondEntity))
                    {
                        HandleCircleCollisionWithBoundary(secondEntity, firstEntity);
                        //Debug.Log("Circle and Wall Collision Check");
                    }
                    if (EntityManager.HasComponent<PlayerComponent>(firstEntity) && EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                    {
                        HandlePlayerCollisionWithProjectile(firstEntity, secondEntity);
                    }
                    if (EntityManager.HasComponent<ProjectileComponent>(firstEntity) && EntityManager.HasComponent<PlayerComponent>(secondEntity))
                    {
                        HandlePlayerCollisionWithProjectile(secondEntity, firstEntity);
                        //Debug.Log("Projectile and Player Collision Check");
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
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius )
            {
                collidingPairs[circleEntity].Add(boundaryEntity);
                Debug.Log("circle entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(circleEntity, boundaryEntity));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
        if(EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius )
            {
                collidingPairs[circleEntity].Add(boundaryEntity);
                Debug.Log("circle entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(circleEntity, boundaryEntity));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
    }

    private void HandlePlayerCollisionWithProjectile(Entity playerEntity, Entity projectileEntity)
    {
        Vector3 playerVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 projectileVector = EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        float firstRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;
        float secondRadius = EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if( (new Vector2(playerVector.x,playerVector.y) - new Vector2(projectileVector.x,projectileVector.y)).magnitude < (firstRadius + secondRadius) )
        {
            collidingPairs[playerEntity].Add(projectileEntity);
            Debug.Log("player entity collide with projectile");
            // HOO BOY
            EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, projectileEntity));
            //EventManager.instance.TriggerEvent(new CollisionEvent(playerEntity, projectileEntity));
        }
    }
}

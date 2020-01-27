using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetectionSystem : ComponentSystem
{
    Game game;

    protected override void OnStartRunning()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
            base.OnStartRunning();
        }
    }

    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            Dictionary<Entity, List<Entity>> checkedPairs = new Dictionary<Entity, List<Entity>>();
            bool skipFlag = false;

            Entities.ForEach((Entity firstEntity, ref Translation xform, ref CollisionComponent collComp) =>
            {
                if (!game.collidingPairs.ContainsKey(firstEntity))
                {
                    game.collidingPairs.Add(firstEntity, new List<Entity>());
                }
                if (!checkedPairs.ContainsKey(firstEntity))
                {
                    checkedPairs.Add(firstEntity, new List<Entity>());
                }

                Entities.ForEach((Entity secondEntity, ref Translation transform, ref CollisionComponent collisionComp) =>
                {
                    if (!game.collidingPairs.ContainsKey(secondEntity))
                    {
                        game.collidingPairs.Add(secondEntity, new List<Entity>());
                    }
                    if (firstEntity == secondEntity)
                    {
                        skipFlag = true;
                    }
                    if (checkedPairs[firstEntity].Contains(secondEntity))
                    {
                        skipFlag = true;
                    }
                    if (EntityManager.HasComponent<PlayerBoundaryComponent>(firstEntity) && EntityManager.HasComponent<PlayerBoundaryComponent>(secondEntity))
                    {
                        skipFlag = true;
                    }
                    if (game.collidingPairs[firstEntity].Contains(secondEntity) || game.collidingPairs[secondEntity].Contains(firstEntity))
                    {
                        skipFlag = true;
                    }

                    if (!skipFlag)
                    {
                    // These internal method calls should instead be exported to a Event/Listener system to handle collision calculations
                    if (EntityManager.HasComponent<PlayerComponent>(firstEntity) && EntityManager.HasComponent<PlayerBoundaryComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithBoundary(game, firstEntity, secondEntity);
                        //Debug.Log("Circle and Wall Collision Check");
                    }
                        if (EntityManager.HasComponent<PlayerBoundaryComponent>(firstEntity) && EntityManager.HasComponent<PlayerComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithBoundary(game, secondEntity, firstEntity);
                        //Debug.Log("Circle and Wall Collision Check");
                    }
                        if (EntityManager.HasComponent<ProjectileComponent>(firstEntity) && EntityManager.HasComponent<ProjectileBoundaryComponent>(secondEntity))
                        {
                            HandleProjectileCollisionWithBoundary(game, firstEntity, secondEntity);
                        }
                        if (EntityManager.HasComponent<ProjectileBoundaryComponent>(firstEntity) && EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                        {
                            HandleProjectileCollisionWithBoundary(game, secondEntity, firstEntity);
                        }
                        if (EntityManager.HasComponent<PlayerComponent>(firstEntity) && EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithProjectile(game, firstEntity, secondEntity);
                        }
                        if (EntityManager.HasComponent<ProjectileComponent>(firstEntity) && EntityManager.HasComponent<PlayerComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithProjectile(game, secondEntity, firstEntity);
                        //Debug.Log("Projectile and Player Collision Check");
                    }
                    }
                    skipFlag = false;
                });
            });
        }
    }

    private void HandleProjectileCollisionWithBoundary(Game game, Entity projectileEntity, Entity boundaryEntity)
    {
        Vector3 circleVector = EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        Vector3 boundaryVector = EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if (EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(circleVector.x, boundaryVector.y);
            if ((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                game.collidingPairs[projectileEntity].Add(boundaryEntity);
                //Debug.Log("Projectile entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(projectileEntity, boundaryEntity));
            }
        }
        if (EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if ((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                game.collidingPairs[projectileEntity].Add(boundaryEntity);
                //Debug.Log("Projectile entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(projectileEntity, boundaryEntity));
            }
        }
    }

    private void HandlePlayerCollisionWithBoundary(Game game, Entity playerEntity, Entity boundaryEntity)
    {
        Vector3 circleVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 boundaryVector = EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;

        if(EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(circleVector.x, boundaryVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius )
            {
                game.collidingPairs[playerEntity].Add(boundaryEntity);
                //Debug.Log("Player entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, boundaryEntity));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
        if(EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius )
            {
                game.collidingPairs[playerEntity].Add(boundaryEntity);
                //Debug.Log("Player entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, boundaryEntity));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
    }

    private void HandlePlayerCollisionWithProjectile(Game game, Entity playerEntity, Entity projectileEntity)
    {
        Vector3 playerVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 projectileVector = EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        float firstRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;
        float secondRadius = EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if( (new Vector2(playerVector.x,playerVector.y) - new Vector2(projectileVector.x,projectileVector.y)).magnitude < (firstRadius + secondRadius) )
        {
            game.collidingPairs[playerEntity].Add(projectileEntity);
            //Debug.Log("player entity collide with projectile");
            // HOO BOY
            EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, projectileEntity));
        }
    }
}

﻿using Assets.MonoScript;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetectionSystem : ComponentSystem
{
    IGame game;
    bool gameInitialized;

    private void Initialize()
    {
        game = (IGame)GameObject.Find("Game").GetComponent(typeof(IGame));
        //gameInitialized = true;
    }

    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
            /*
            if (!gameInitialized)
                Initialize();
            */
            Initialize();
            Dictionary<Entity, List<Entity>> checkedPairs = new Dictionary<Entity, List<Entity>>();
            bool skipFlag = false;

            Entities.ForEach((Entity firstEntity, ref Translation xform, ref CollisionComponent collComp) =>
            {
                if (!game.GetCollidingPairs().ContainsKey(firstEntity))
                {
                    game.GetCollidingPairs().Add(firstEntity, new List<Entity>());
                }
                if (!checkedPairs.ContainsKey(firstEntity))
                {
                    checkedPairs.Add(firstEntity, new List<Entity>());
                }

                Entities.ForEach((Entity secondEntity, ref Translation transform, ref CollisionComponent collisionComp) =>
                {
                    if (!game.GetCollidingPairs().ContainsKey(secondEntity))
                    {
                        game.GetCollidingPairs().Add(secondEntity, new List<Entity>());
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
                    if (game.GetCollidingPairs()[firstEntity].Contains(secondEntity) || game.GetCollidingPairs()[secondEntity].Contains(firstEntity))
                    {
                        skipFlag = true;
                    }

                    if (!skipFlag)
                    {
                        // These internal method calls should instead be exported to a Event/Listener system to handle collision calculations
                        if (EntityManager.HasComponent<PlayerComponent>(firstEntity) && EntityManager.HasComponent<PlayerBoundaryComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithBoundary(game, firstEntity, secondEntity);
                            Debug.Log("Player and Player Boundary Collision Check");
                        }
                        else if (EntityManager.HasComponent<PlayerBoundaryComponent>(firstEntity) && EntityManager.HasComponent<PlayerComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithBoundary(game, secondEntity, firstEntity);
                            Debug.Log("Player Boundary and Player Collision Check");
                        }
                        else if (EntityManager.HasComponent<ProjectileComponent>(firstEntity) && EntityManager.HasComponent<ProjectileBoundaryComponent>(secondEntity))
                        {
                            HandleProjectileCollisionWithBoundary(game, firstEntity, secondEntity);
                            Debug.Log("Projectile and Projectile Boundary Collision Check");
                        }
                        else if (EntityManager.HasComponent<ProjectileBoundaryComponent>(firstEntity) && EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                        {
                            HandleProjectileCollisionWithBoundary(game, secondEntity, firstEntity);
                            Debug.Log("Projectile Boundary and Projectile Collision Check");
                        }
                        else if (EntityManager.HasComponent<PlayerComponent>(firstEntity) && EntityManager.HasComponent<ProjectileComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithProjectile(game, firstEntity, secondEntity);
                            Debug.Log("Player and Projectile Collision Check");
                        }
                        else if (EntityManager.HasComponent<ProjectileComponent>(firstEntity) && EntityManager.HasComponent<PlayerComponent>(secondEntity))
                        {
                            HandlePlayerCollisionWithProjectile(game, secondEntity, firstEntity);
                            Debug.Log("Projectile and Player Collision Check");
                        }
                    }
                    skipFlag = false;
                });
            });
        }
    }

    private void HandleProjectileCollisionWithBoundary(IGame game, Entity projectileEntity, Entity boundaryEntity)
    {
        Vector3 circleVector = EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        Vector3 boundaryVector = EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if (EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(circleVector.x, boundaryVector.y);
            if ((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                game.GetCollidingPairs()[projectileEntity].Add(boundaryEntity);
                Debug.Log("Projectile entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(projectileEntity, boundaryEntity));
            }
        }

        if (EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if ((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                game.GetCollidingPairs()[projectileEntity].Add(boundaryEntity);
                //Debug.Log("Projectile entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(projectileEntity, boundaryEntity));
            }
        }
    }

    private void HandlePlayerCollisionWithBoundary(IGame game, Entity playerEntity, Entity boundaryEntity)
    {
        Vector3 circleVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 boundaryVector = EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;

        if(EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(circleVector.x, boundaryVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius )
            {
                game.GetCollidingPairs()[playerEntity].Add(boundaryEntity);
                Debug.Log("Player entity collide with boundary");
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
                game.GetCollidingPairs()[playerEntity].Add(boundaryEntity);
                Debug.Log("Player entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, boundaryEntity));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
    }

    private void HandlePlayerCollisionWithProjectile(IGame game, Entity playerEntity, Entity projectileEntity)
    {
        Vector3 playerVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 projectileVector = EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        float firstRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;
        float secondRadius = EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if( (new Vector2(playerVector.x,playerVector.y) - new Vector2(projectileVector.x,projectileVector.y)).magnitude < (firstRadius + secondRadius) )
        {
            game.GetCollidingPairs()[playerEntity].Add(projectileEntity);
            Debug.Log("player entity collide with projectile");
            // HOO BOY
            EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, projectileEntity));
        }
    }
}

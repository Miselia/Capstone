using Assets.MonoScript;
using Assets.Resources;
using Assets.Systems;
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
        Initialize();
        int collisionCounter = 0;
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

        int parentInt = World.Active.EntityManager.GetComponentData<QuadTreeReferenceComponent>(firstEntity).parentID;
        QuadTreeNode parent = QuadTreeSystem.quadTreeDict[parentInt];
        byte firstMask = EntityManager.GetComponentData<CollisionComponent>(firstEntity).mask;

        while (parent != null)
        {
            byte secondMask;
            List<Entity> leaves = parent.leaves;

            foreach (Entity secondEntity in leaves)
            {                
                secondMask = EntityManager.GetComponentData<CollisionComponent>(secondEntity).mask;
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
                if (EntityManager.HasComponent<IsBoundaryComponent>(firstEntity) && EntityManager.HasComponent<IsBoundaryComponent>(secondEntity))
                {
                    skipFlag = true;
                }
                if ((firstMask & secondMask) == 0)
                {
                    skipFlag = true;
                    //Debug.Log("Skipped due to incompatible byte comparison");
                }
                if (firstMask == secondMask)
                {
                    skipFlag = true;
                    // Skipped because two of the same type should never collide
                }
                if (game.GetCollidingPairs()[firstEntity].Contains(secondEntity) || game.GetCollidingPairs()[secondEntity].Contains(firstEntity))
                {
                    skipFlag = true;
                }

                if (!skipFlag)
                {
                    collisionCounter++;
                    int compare = firstMask & secondMask;
                    switch(compare)
                    {
                        case 1:
                            // Player x Projectile Collision
                            if(EntityManager.HasComponent<PlayerComponent>(firstEntity))
                                HandlePlayerCollisionWithProjectile(game, firstEntity, secondEntity, compare);
                            else
                            {
                                if(EntityManager.HasComponent<PlayerComponent>(secondEntity))
                                    HandlePlayerCollisionWithProjectile(game, secondEntity, firstEntity, compare);
                            }
                            break;
                        case 2:
                            // Projectile x Projectile Boundary Collision
                            if(EntityManager.HasComponent<ProjectileComponent>(firstEntity))
                                HandleProjectileCollisionWithBoundary(game, firstEntity, secondEntity, compare);
                            else
                                HandleProjectileCollisionWithBoundary(game, secondEntity, firstEntity, compare);
                            break;
                        case 4:
                            // Player x Player Boundary Collision
                            if(EntityManager.HasComponent<PlayerComponent>(firstEntity))
                                HandlePlayerCollisionWithBoundary(game, firstEntity, secondEntity, compare);
                            else
                                HandlePlayerCollisionWithBoundary(game, secondEntity, firstEntity, compare);
                            break;
                        case 8:
                            // Projectile x Player Boundary Collision
                            Entity projectile;
                            Entity bound;
                            if (EntityManager.HasComponent<ProjectileComponent>(firstEntity))
                            {
                                projectile = firstEntity;
                                bound = secondEntity;
                            }
                            else
                            {
                                projectile = secondEntity;
                                bound = firstEntity;
                            }
                            // ****** IF YOU EVER HAVE ISSUES WITH COLLISIONS WITH PROJECTILES AND PLAYER BOUNDARIES CHECK HERE ******** \\
                            // Switch "cases" cannot be evaluated and must be declared, meaning that we cannot reference Constants
                            // Make sure these matches match or else the code will break
                            switch(EntityManager.GetComponentData<ProjectileCollisionWithPlayerBoundaryComponent>(projectile).caseInt)
                            {
                                case 8:
                                    HandleGearCollisionWithBoundary(game, projectile, bound, Constants.GearID);
                                    break;
                                // case 9 is cigar
                                // case 10 is rocket
                            }
                            break;
                    }
                }
                skipFlag = false;
            }
            parent = parent.parent;
        }
        });
        
    }

    private void HandleGearCollisionWithBoundary(IGame game, Entity gearEntity, Entity playerBoundEntity, int mask)
    {
        //Debug.Log("Gear collided with Player Boundary");
        Vector3 circleVector = EntityManager.GetComponentData<Translation>(gearEntity).Value;
        Vector3 boundaryVector = EntityManager.GetComponentData<Translation>(playerBoundEntity).Value;
        float circleRadius = EntityManager.GetComponentData<CollisionComponent>(gearEntity).collisionRadius;

        if (EntityManager.GetComponentData<PlayerBoundaryComponent>(playerBoundEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(circleVector.x, boundaryVector.y);
            if ((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                game.GetCollidingPairs()[gearEntity].Add(playerBoundEntity);
                Debug.Log("Gear collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(gearEntity, playerBoundEntity, mask));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
        if (EntityManager.GetComponentData<PlayerBoundaryComponent>(playerBoundEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if ((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius)
            {
                game.GetCollidingPairs()[gearEntity].Add(playerBoundEntity);
                Debug.Log("Gear collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(gearEntity, playerBoundEntity, mask));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
    }

    private void HandleProjectileCollisionWithBoundary(IGame game, Entity projectileEntity, Entity boundaryEntity, int mask)
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
                //Debug.Log("Projectile entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(projectileEntity, boundaryEntity, mask));
                Debug.Log("Collision with Normal.x = 0");
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
                EventManager.instance.QueueEvent(new CollisionEvent(projectileEntity, boundaryEntity, mask));
                Debug.Log("Collision with Normal.y = 0");
            }
        }
    }

    private void HandlePlayerCollisionWithBoundary(IGame game, Entity playerEntity, Entity boundaryEntity, int mask)
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
                //Debug.Log("Player entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, boundaryEntity, mask));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
        if(EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, circleVector.y);
            if((nearestWallPosition - new Vector2(circleVector.x, circleVector.y)).magnitude < circleRadius )
            {
                game.GetCollidingPairs()[playerEntity].Add(boundaryEntity);
                //Debug.Log("Player entity collide with boundary");
                // HOO BOY
                EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, boundaryEntity, mask));
                //EventManager.instance.TriggerEvent(new CollisionEvent(circleEntity, boundaryEntity));
            }
        }
    }

    private void HandlePlayerCollisionWithProjectile(IGame game, Entity playerEntity, Entity projectileEntity, int mask)
    {
        Vector3 playerVector = EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 projectileVector = EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        float firstRadius = EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;
        float secondRadius = EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if( (new Vector2(playerVector.x,playerVector.y) - new Vector2(projectileVector.x,projectileVector.y)).magnitude < (firstRadius + secondRadius) )
        {
            game.GetCollidingPairs()[playerEntity].Add(projectileEntity);
            //Debug.Log("player entity collide with projectile");
            // HOO BOY
            EventManager.instance.QueueEvent(new CollisionEvent(playerEntity, projectileEntity, mask));
        }
    }
}

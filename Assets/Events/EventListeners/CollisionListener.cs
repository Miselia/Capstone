﻿using Assets.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CollisionListener : MonoBehaviour, IGenericEventListener
{
    /* Due to having no other idea how to do this, Projectile Collisions with Player Boundaries will have to have their own
     * values passed in, meaning there will be yet another Integer key here. Yay! ... ...
     * Gear = 8
     * Cigar = 9
     * Rocket = 10
     * */
    public bool HandleEvent(IGenericEvent evt)
    {
        if(evt is CollisionEvent)
        {
            CollisionEvent ce = evt as CollisionEvent;

            // Signifies Player x Projectile Collision
            if(ce.collisionMask == 1)
            {
                //Debug.Log("Player x Projectile Collision before in CollisionListener");
                PlayerProjectileCollisionHelper(ce.entityA, ce.entityB);
                //Debug.Log("Player x Projectile Collision after in CollisionListener");
                return true;
            }
            // Projectile x Boundary Collision
            if (ce.collisionMask == 2)
            {
                ProjectileBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Player x Boundary Collision
            if (ce.collisionMask == 4)
            {
                PlayerBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Gear x Boundary Collision
            if (ce.collisionMask == Constants.GearID)
            {
                GearBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Cigar x Boundary Colliison
            if (ce.collisionMask == Constants.CigarID)
            {
                Debug.Log("Handle Cigar collision with Player Boundary");
                return true;
            }
            // Rocket x Boundary Collision
            /*if (ce.collisionMask == Constants.RocketID)
            {
                Debug.Log("Handle Rocket collision with Player Boundary");
                return true;
            }*/
        }
        return false;
    }

    private bool GearBoundaryCollisionHelper(Entity gearEntity, Entity playBoundEntity)
    {
        bool exists = World.Active.EntityManager.Exists(gearEntity) && World.Active.EntityManager.Exists(playBoundEntity);
        if (exists)
        {
            Vector3 playerVector = World.Active.EntityManager.GetComponentData<Translation>(gearEntity).Value;
            Vector3 boundaryVector = World.Active.EntityManager.GetComponentData<Translation>(playBoundEntity).Value;
            float circleRadius = World.Active.EntityManager.GetComponentData<CollisionComponent>(gearEntity).collisionRadius;
            Vector2 boundNormal = World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(playBoundEntity).Normal;

            if (boundNormal.x == 0)
            {
                Vector2 nearestWallPosition = new Vector2(playerVector.x, boundaryVector.y);
                playerVector.y = nearestWallPosition.y + boundNormal.y * circleRadius;
                World.Active.EntityManager.SetComponentData<Translation>(gearEntity,
                    new Translation { Value = new Unity.Mathematics.float3(playerVector.x, playerVector.y, playerVector.z) });

                //Debug.Log("Gear position and movement readjusted");
                World.Active.EntityManager.SetComponentData<MovementComponent>(gearEntity,
                    new MovementComponent { movementVector = new Vector2(-boundNormal.y * 5, 0) });
                EventManager.instance.QueueEvent(new EndCollisionEvent(gearEntity, playBoundEntity));
            }
            if (boundNormal.y == 0)
            {
                Vector2 nearestWallPosition = new Vector2(boundaryVector.x, playerVector.y);
                playerVector.x = nearestWallPosition.x + boundNormal.x * circleRadius;
                World.Active.EntityManager.SetComponentData<Translation>(gearEntity,
                    new Translation { Value = new Unity.Mathematics.float3(playerVector.x, playerVector.y, playerVector.z) });

                //Debug.Log("Gear position and movement readjusted");
                World.Active.EntityManager.SetComponentData<MovementComponent>(gearEntity,
                    new MovementComponent { movementVector = new Vector2(0, boundNormal.x * 5) });
                EventManager.instance.QueueEvent(new EndCollisionEvent(gearEntity, playBoundEntity));
            }
            return true;
        }
        return false;
    }

    private bool ProjectileBoundaryCollisionHelper(Entity projectileEntity, Entity boundaryEntity)
    {
        //Debug.Log("Projectile collision with projectile boundary");
        bool exists = World.Active.EntityManager.Exists(projectileEntity);

        if (exists)
        {
            Vector3 projectileVector = World.Active.EntityManager.GetComponentData<Translation>(projectileEntity).Value;
            Vector3 boundaryVector = World.Active.EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
            float circleRadius = World.Active.EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

            if (World.Active.EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.x == 0)
            {
                Vector2 nearestWallPosition = new Vector2(projectileVector.x, boundaryVector.y);
                projectileVector.y = nearestWallPosition.y + circleRadius;
                World.Active.EntityManager.SetComponentData<MovementComponent>(projectileEntity, new MovementComponent(new Vector2(0, 0)));
                //Debug.Log("Projectile position readjusted");
                EventManager.instance.QueueEvent(new EndCollisionEvent(projectileEntity, boundaryEntity));
                World.Active.EntityManager.AddComponent(projectileEntity, typeof(DeleteComp));
                World.Active.EntityManager.SetComponentData(projectileEntity, new DeleteComp(0));
            }
            if (World.Active.EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.y == 0)
            {
                Vector2 nearestWallPosition = new Vector2(boundaryVector.x, projectileVector.y);
                projectileVector.x = nearestWallPosition.x + circleRadius;
                World.Active.EntityManager.SetComponentData<MovementComponent>(projectileEntity, new MovementComponent(new Vector2(0, 0)));
                //Debug.Log("Projectile position readjusted");
                EventManager.instance.QueueEvent(new EndCollisionEvent(projectileEntity, boundaryEntity));
                World.Active.EntityManager.AddComponent(projectileEntity, typeof(DeleteComp));
                World.Active.EntityManager.SetComponentData(projectileEntity, new DeleteComp(0));
            }
            return true;
        }
        //Debug.Log("Does projectile entity exist: " + exists);
        return false;
    }

    private bool PlayerBoundaryCollisionHelper(Entity playerEntity, Entity boundaryEntity)
    {
        bool exists = World.Active.EntityManager.Exists(playerEntity) && World.Active.EntityManager.Exists(boundaryEntity);
        if (exists)
        {
            Vector3 playerVector = World.Active.EntityManager.GetComponentData<Translation>(playerEntity).Value;
            Vector3 boundaryVector = World.Active.EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
            float circleRadius = World.Active.EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;

            if (World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.x == 0)
            {
                Vector2 nearestWallPosition = new Vector2(playerVector.x, boundaryVector.y);
                playerVector.y = nearestWallPosition.y + World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.y * circleRadius;
                World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                    new Translation { Value = new Unity.Mathematics.float3(playerVector.x, playerVector.y, playerVector.z) });
                /*World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                    new Translation { Value = new Unity.Mathematics.float3(0, 0, 0) });*/
                //Debug.Log("Player position readjusted");
                EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, boundaryEntity));
            }
            if (World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.y == 0)
            {
                Vector2 nearestWallPosition = new Vector2(boundaryVector.x, playerVector.y);
                playerVector.x = nearestWallPosition.x + World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.x * circleRadius;
                World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                    new Translation { Value = new Unity.Mathematics.float3(playerVector.x, playerVector.y, playerVector.z) });
                /*World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                    new Translation { Value = new Unity.Mathematics.float3(0, 0, 0) });*/
                //Debug.Log("Player position readjusted");
                EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, boundaryEntity));
            }
            return true;
        }
        return false;
    }

    private bool PlayerProjectileCollisionHelper(Entity playerEntity, Entity projectileEntity)
    {
        /*
        int[] values = World.Active.EntityManager.GetComponentData<PlayerComponent>(playerEntity).LoseHealth(Constants.DefaultProjectileDamage);
        Debug.Log("Player Health after collision w/ projectile: " + values[0]);
        EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, projectileEntity));
        EventManager.instance.QueueEvent(new UIUpdateEvent(values[0], values[1], values[2]));
        */
        bool exists = World.Active.EntityManager.Exists(playerEntity) && World.Active.EntityManager.Exists(projectileEntity);
        if (exists)
        {
            if (World.Active.EntityManager.HasComponent<ManaRegenBuffComp>(projectileEntity))
            {
                World.Active.EntityManager.AddComponent(playerEntity, typeof(IsBuffedComponent));
                World.Active.EntityManager.AddComponent(playerEntity, typeof(ManaRegenBuffComp));
                World.Active.EntityManager.SetComponentData(playerEntity, World.Active.EntityManager.GetComponentData<ManaRegenBuffComp>(projectileEntity));
            }

            int projectileDamage = World.Active.EntityManager.GetComponentData<ProjectileComponent>(projectileEntity).damage;
            World.Active.EntityManager.AddComponent(playerEntity, typeof(HealthDeltaComp));
            World.Active.EntityManager.SetComponentData(playerEntity, new HealthDeltaComp(projectileDamage));
            World.Active.EntityManager.AddComponent(projectileEntity, typeof(DeleteComp));
            World.Active.EntityManager.SetComponentData(projectileEntity, new DeleteComp(0));
            //World.Active.EntityManager.SetComponentData(projectileEntity, new DeleteComp(60));
            return true;
        }
        return false;
    }

    private void Start()
    {
        EventManager.instance.RegisterListener<CollisionEvent>(this);
    }
}

using Assets.Resources;
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
    public bool HandleEvent(IGenericEvent evt)
    {
        if(evt is CollisionEvent)
        {
            CollisionEvent ce = evt as CollisionEvent;

            // Signifies Player x Projectile Collision
            if(World.Active.EntityManager.HasComponent<PlayerComponent>(ce.entityA) &&
                World.Active.EntityManager.HasComponent<ProjectileComponent>(ce.entityB))
            {
                //Debug.Log("Player x Projectile Collision before in CollisionListener");
                PlayerProjectileCollisionHelper(ce.entityA, ce.entityB);
                //Debug.Log("Player x Projectile Collision after in CollisionListener");
                return true;
            }
            // Player x Boundary Collision
            if(World.Active.EntityManager.HasComponent<PlayerComponent>(ce.entityA) &&
                World.Active.EntityManager.HasComponent<PlayerBoundaryComponent>(ce.entityB))
            {
                PlayerBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Projectile x Boundary Collision
            if(World.Active.EntityManager.HasComponent<ProjectileComponent>(ce.entityA) &&
                World.Active.EntityManager.HasComponent<ProjectileBoundaryComponent>(ce.entityB))
            {
                ProjectileBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
        }
        return false;
    }

    private bool ProjectileBoundaryCollisionHelper(Entity projectileEntity, Entity boundaryEntity)
    {
        Vector3 projectileVector = World.Active.EntityManager.GetComponentData<Translation>(projectileEntity).Value;
        Vector3 boundaryVector = World.Active.EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = World.Active.EntityManager.GetComponentData<CollisionComponent>(projectileEntity).collisionRadius;

        if (World.Active.EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(projectileVector.x, boundaryVector.y);
            projectileVector.y = nearestWallPosition.y + circleRadius;
            World.Active.EntityManager.SetComponentData<MovementComponent>(projectileEntity, new MovementComponent(new Vector2(0, 0)));
            Debug.Log("Projectile position readjusted");
            EventManager.instance.QueueEvent(new EndCollisionEvent(projectileEntity, boundaryEntity));
            World.Active.EntityManager.AddComponent(projectileEntity, typeof(DeleteComp));
        }
        if (World.Active.EntityManager.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, projectileVector.y);
            projectileVector.x = nearestWallPosition.x + circleRadius;
            World.Active.EntityManager.SetComponentData<MovementComponent>(projectileEntity, new MovementComponent(new Vector2(0, 0)));
            Debug.Log("Projectile position readjusted");
            EventManager.instance.QueueEvent(new EndCollisionEvent(projectileEntity, boundaryEntity));
            World.Active.EntityManager.AddComponent(projectileEntity, typeof(DeleteComp));
        }
        return true;
    }

    private bool PlayerBoundaryCollisionHelper(Entity playerEntity, Entity boundaryEntity)
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
            Debug.Log("Player position readjusted");
            EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, boundaryEntity));
        }
        if(World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, playerVector.y);
            playerVector.x = nearestWallPosition.x + World.Active.EntityManager.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.x * circleRadius;
            World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                new Translation { Value = new Unity.Mathematics.float3(playerVector.x, playerVector.y, playerVector.z) });
            /*World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                new Translation { Value = new Unity.Mathematics.float3(0, 0, 0) });*/
            Debug.Log("Player position readjusted");
            EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, boundaryEntity));
        }

        return true;
    }

    private bool PlayerProjectileCollisionHelper(Entity playerEntity, Entity projectileEntity)
    {
        /*
        int[] values = World.Active.EntityManager.GetComponentData<PlayerComponent>(playerEntity).LoseHealth(Constants.DefaultProjectileDamage);
        Debug.Log("Player Health after collision w/ projectile: " + values[0]);
        EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, projectileEntity));
        EventManager.instance.QueueEvent(new UIUpdateEvent(values[0], values[1], values[2]));
        */
        World.Active.EntityManager.AddComponent(playerEntity, typeof(HealthDeltaComp));
        World.Active.EntityManager.SetComponentData(playerEntity, new HealthDeltaComp(-Constants.DefaultProjectileDamage));
        World.Active.EntityManager.AddComponent(projectileEntity, typeof(DeleteComp));
        //World.Active.EntityManager.SetComponentData(projectileEntity, new DeleteComp(60));
        return true;
    }

    private void Start()
    {
        EventManager.instance.RegisterListener<CollisionEvent>(this);
    }
}

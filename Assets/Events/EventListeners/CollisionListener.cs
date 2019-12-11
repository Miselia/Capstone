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
                PlayerProjectileCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Player x Boundary Collision
            if(World.Active.EntityManager.HasComponent<PlayerComponent>(ce.entityA) &&
                World.Active.EntityManager.HasComponent<BoundaryComponent>(ce.entityB))
            {
                PlayerBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Projectile x Boundary Collision
            if(World.Active.EntityManager.HasComponent<ProjectileComponent>(ce.entityA) &&
                World.Active.EntityManager.HasComponent<BoundaryComponent>(ce.entityB))
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

        if (World.Active.EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(projectileVector.x, boundaryVector.y);
            projectileVector.y = nearestWallPosition.y + circleRadius;
            World.Active.EntityManager.SetComponentData<MovementComponent>(projectileEntity, new MovementComponent(new Vector2(0, 0)));
            Debug.Log("Projectile position readjusted");
        }
        if (World.Active.EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, projectileVector.y);
            projectileVector.x = nearestWallPosition.x + circleRadius;
            World.Active.EntityManager.SetComponentData<MovementComponent>(projectileEntity, new MovementComponent(new Vector2(0, 0)));
            Debug.Log("Projectile position readjusted");
        }
        return true;
    }

    private bool PlayerBoundaryCollisionHelper(Entity playerEntity, Entity boundaryEntity)
    {
        Vector3 playerVector = World.Active.EntityManager.GetComponentData<Translation>(playerEntity).Value;
        Vector3 boundaryVector = World.Active.EntityManager.GetComponentData<Translation>(boundaryEntity).Value;
        float circleRadius = World.Active.EntityManager.GetComponentData<CollisionComponent>(playerEntity).collisionRadius;

        if (World.Active.EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.x == 0)
        {
            Vector2 nearestWallPosition = new Vector2(playerVector.x, boundaryVector.y);
            playerVector.y = nearestWallPosition.y + circleRadius;
            Debug.Log("Player position readjusted");
        }
        if(World.Active.EntityManager.GetComponentData<BoundaryComponent>(boundaryEntity).Normal.y == 0)
        {
            Vector2 nearestWallPosition = new Vector2(boundaryVector.x, playerVector.y);
            playerVector.x = nearestWallPosition.x + circleRadius;
            World.Active.EntityManager.SetComponentData<Translation>(playerEntity,
                new Translation { Value = new Unity.Mathematics.float3(playerVector.x, playerVector.y, playerVector.z) });
            Debug.Log("Player position readjusted");
        }

        return true;
    }

    private bool PlayerProjectileCollisionHelper(Entity player, Entity projectile)
    {
        World.Active.EntityManager.SetComponentData<MovementComponent>(projectile, new MovementComponent(new Vector2(0,0))); 
        return true;
    }

    private void Start()
    {
        EventManager.instance.RegisterListener<CollisionEvent>(this);
    }
}

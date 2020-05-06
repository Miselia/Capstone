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
    private EntityManager em;
    void Start()
    {
        em = World.Active.EntityManager;
        EventManager.instance.RegisterListener<CollisionEvent>(this);
    }

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
            else if (ce.collisionMask == 2)
            {
                ProjectileBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Player x Boundary Collision
            else if (ce.collisionMask == 4)
            {
                PlayerBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Gear x Boundary Collision
            else if (ce.collisionMask == Constants.GearID)
            {
                Debug.Log("Collision Between Gear and Player Bound");
                GearBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            // Cigar x Boundary Colliison
            else if (ce.collisionMask == Constants.CigarID)
            {
                // Check to see if the normal of the boundary passed in is (0, 1) aka the bottom boundary, skip otherwise
                if (em.GetComponentData<PlayerBoundaryComponent>(ce.entityB).Normal == new Vector2(0, 1))
                {
                    CigarBoundaryCollisionHelpler(ce.entityA, ce.entityB);
                    return true;
                }
                else
                    Debug.Log("Collision between Cigar and Boundary skipped due to normal != (0,1)");
                return false;
            }
            else if (ce.collisionMask == Constants.BettyID)
            {
                Debug.Log("Handle Betty collision with Player Boundary");
                // Possible that just adding a "BouncingHelper" method is preferable since this is all the Bouncing Betty will do anyways
                BettyBoundaryCollisionHelper(ce.entityA, ce.entityB);
                return true;
            }
            else if (ce.collisionMask == Constants.HailID)
            {
                Debug.Log("Handle Hail collisoin with Player Boundary");
                HailCollisionBoundaryHelpler(ce.entityA, ce.entityB);
            }
            // Rocket x Boundary Collision, might not actually collide with player boundaries for now (balance before implementation)
            /*if (ce.collisionMask == Constants.RocketID)
            {
                Debug.Log("Handle Rocket collision with Player Boundary");
                return true;
            }*/
        }
        return false;
    }

    private void HailCollisionBoundaryHelpler(Entity hailEntity, Entity playBoundEntity)
    {
        bool exists = em.Exists(hailEntity) && em.Exists(playBoundEntity);
        if (exists)
        {
            // Once hail reaches the wall, adjust position, stop movement, and refresh the movementSpeed debuff
            RepositionCollisionWithNearestBoundary(hailEntity, playBoundEntity);
            em.AddComponent(hailEntity, typeof(DeleteComp));
            em.SetComponentData<DeleteComp>(hailEntity, new DeleteComp(5));
            em.SetComponentData<MovementSpeedBuffComp>(hailEntity, new MovementSpeedBuffComp(0.5f, 5));
            em.SetComponentData<MovementComponent>(hailEntity, new MovementComponent(new Vector2()));
        }
    }

    private Vector2 RepositionCollisionWithNearestBoundary(Entity circle, Entity boundary)
    {
        Vector2 resetPos;

        Vector3 circlePos = em.GetComponentData<Translation>(circle).Value;
        Vector3 boundPos = em.GetComponentData<Translation>(boundary).Value;
        float circleRadius = em.GetComponentData<CollisionComponent>(circle).collisionRadius;
        Vector2 boundNormal = em.GetComponentData<PlayerBoundaryComponent>(boundary).Normal;

        if (boundNormal.x == 0)
        {
            // If horizontal boundary, nearestPos = ( x val of projectile, y value of boundary - (radius +/- 1) ) depending on normal collided with
            resetPos = new Vector2(circlePos.x, boundPos.y + boundNormal.y * (circleRadius + .1f));
            em.SetComponentData<Translation>(circle, new Translation { Value = new Unity.Mathematics.float3(resetPos.x, resetPos.y, 0) });
        }
        else
        {
            // If vertical boundary, nearest pos = ( y val of projectile, x value of boundary + (radius +/- 1) ) depending on normal collided with
            resetPos = new Vector2(boundPos.x + boundNormal.x * (circleRadius + .1f), circlePos.y);
            em.SetComponentData<Translation>(circle, new Translation { Value = new Unity.Mathematics.float3(resetPos.x, resetPos.y, 0) });
        }

        return resetPos;
    }

    private void BettyBoundaryCollisionHelper(Entity bettyEntity, Entity playBoundEntity)
    {
        // Currently Bounding Betty doesn't work because the projectile is colliding with both top bounds because bounds are infinite length
        bool exists = em.Exists(bettyEntity) && em.Exists(playBoundEntity);
        if (exists)
        {
            RepositionCollisionWithNearestBoundary(bettyEntity, playBoundEntity);

            Vector2 boundNormal = em.GetComponentData<PlayerBoundaryComponent>(playBoundEntity).Normal;
            Vector2 bettyMove = em.GetComponentData<MovementComponent>(bettyEntity).movementVector;

            if (boundNormal.x == 0)
            {
                // If normal.X = 0, then this is a horizontal bound, with y = -1 or y = 1
                // Perfect reflection means just flipping the value that corresponds to the normal of the boundary collided with
                em.SetComponentData<MovementComponent>(bettyEntity,
                    new MovementComponent { movementVector = new Vector2(bettyMove.x, bettyMove.y * -1), multiplier = 1 });

                EventManager.instance.QueueEvent(new EndCollisionEvent(bettyEntity, playBoundEntity));
            }
            if (boundNormal.y == 0)
            {
                // If normal.Y = 0, then this is vertical bound, with x = 1 or x = -1
                em.SetComponentData<MovementComponent>(bettyEntity,
                    new MovementComponent { movementVector = new Vector2(bettyMove.x * -1, bettyMove.y), multiplier = 1 });

                EventManager.instance.QueueEvent(new EndCollisionEvent(bettyEntity, playBoundEntity));
            }
        }
    }

    private bool GearBoundaryCollisionHelper(Entity gearEntity, Entity playBoundEntity)
    {
        //Debug.Log("Gear Collision with Player Boundary");
        bool exists = em.Exists(gearEntity) && em.Exists(playBoundEntity);
        if (exists)
        {
            RepositionCollisionWithNearestBoundary(gearEntity, playBoundEntity);

            Vector2 boundNormal = em.GetComponentData<PlayerBoundaryComponent>(playBoundEntity).Normal;

            if (boundNormal.x == 0)
            {
                em.SetComponentData<MovementComponent>(gearEntity,
                    new MovementComponent { movementVector = new Vector2(-boundNormal.y * 5, 0), multiplier = 1 }) ;

                EventManager.instance.QueueEvent(new EndCollisionEvent(gearEntity, playBoundEntity));
            }
            if (boundNormal.y == 0)
            {
                em.SetComponentData<MovementComponent>(gearEntity,
                    new MovementComponent { movementVector = new Vector2(0, boundNormal.x * 5), multiplier = 1 });

                EventManager.instance.QueueEvent(new EndCollisionEvent(gearEntity, playBoundEntity));
            }
            return true;
        }
        return false;
    }

    private bool CigarBoundaryCollisionHelpler(Entity cigar, Entity playBoundEntity)
    {
        bool exists = em.Exists(cigar) && em.Exists(playBoundEntity);
        if (exists)
        {
            Vector2 cigarPos = new Vector2(em.GetComponentData<Translation>(cigar).Value.x,
                                           em.GetComponentData<Translation>(cigar).Value.y);

            em.DestroyEntity(cigar);
            // Because this is a Boundary and not a Projectile, we will use the "damage/d" variable as our "side" salue
            int side = (cigarPos.x < 0) ? -1 : -2;
            EventManager.instance.QueueEvent(new CreateProjectileEvent("smashCigar", side, cigarPos, new Vector2(-1, 0), 1f, 0));
            return true;
        }
        return false;
    }

    private bool ProjectileBoundaryCollisionHelper(Entity projectileEntity, Entity boundaryEntity)
    {
        //Debug.Log("Projectile collision with projectile boundary");
        bool exists = em.Exists(projectileEntity);

        if (exists)
        {
            if (em.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.x == 0)
            {
                EventManager.instance.QueueEvent(new EndCollisionEvent(projectileEntity, boundaryEntity));
                em.AddComponent(projectileEntity, typeof(DeleteComp));
                em.SetComponentData(projectileEntity, new DeleteComp(0));
            }
            if (em.GetComponentData<ProjectileBoundaryComponent>(boundaryEntity).Normal.y == 0)
            {
                EventManager.instance.QueueEvent(new EndCollisionEvent(projectileEntity, boundaryEntity));
                em.AddComponent(projectileEntity, typeof(DeleteComp));
                em.SetComponentData(projectileEntity, new DeleteComp(0));
            }
            return true;
        }
        return false;
    }

    private bool PlayerBoundaryCollisionHelper(Entity playerEntity, Entity boundaryEntity)
    {
        bool exists = em.Exists(playerEntity) && em.Exists(boundaryEntity);
        if (exists)
        {
            RepositionCollisionWithNearestBoundary(playerEntity, boundaryEntity);

            if (em.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.x == 0)
            {
                EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, boundaryEntity));
            }
            if (em.GetComponentData<PlayerBoundaryComponent>(boundaryEntity).Normal.y == 0)
            {
                EventManager.instance.QueueEvent(new EndCollisionEvent(playerEntity, boundaryEntity));
            }
            return true;
        }
        return false;
    }

    private bool PlayerProjectileCollisionHelper(Entity playerEntity, Entity projectileEntity)
    {
        bool exists = em.Exists(playerEntity) && em.Exists(projectileEntity);
        if (exists)
        {
            if (em.HasComponent<ManaRegenBuffComp>(projectileEntity))
            {
                em.AddComponent(playerEntity, typeof(IsBuffedComponent));
                em.AddComponent(playerEntity, typeof(ManaRegenBuffComp));
                // Mana regen only ticks down when applied to a player, meaning we can just copy of the component as is
                em.SetComponentData(playerEntity, em.GetComponentData<ManaRegenBuffComp>(projectileEntity));
            }

            if (em.HasComponent<MovementSpeedBuffComp>(projectileEntity))
            {
                em.AddComponent(playerEntity, typeof(IsBuffedComponent));
                em.AddComponent(playerEntity, typeof(MovementSpeedBuffComp));
                // For move speed buffs we instead need to reset the values of the component because the timer has been reducing over time due to MovementSpeedBuffComp being generic
                em.SetComponentData(playerEntity, new MovementSpeedBuffComp(em.GetComponentData<MovementSpeedBuffComp>(projectileEntity).value, em.GetComponentData<MovementSpeedBuffComp>(projectileEntity).maxTimer));
            }

            int projectileDamage = em.GetComponentData<ProjectileComponent>(projectileEntity).damage;
            em.AddComponent(playerEntity, typeof(HealthDeltaComp));
            em.SetComponentData(playerEntity, new HealthDeltaComp(projectileDamage));
            if(em.HasComponent<DeleteComp>(projectileEntity))
            {
                em.SetComponentData<DeleteComp>(projectileEntity, new DeleteComp());
            }
            else
            {
                em.AddComponent(projectileEntity, typeof(DeleteComp));
            }
            return true;
        }
        return false;
    }
}

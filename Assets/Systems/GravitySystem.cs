using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;
using Unity.Transforms;

public class GravitySystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        
            Entities.ForEach((Entity e, ref GravityComponent gravity, ref Translation wellPosition) =>
            {
                float str = gravity.strength;
                float rad = gravity.radius;
                Vector2 pos = new Vector2(wellPosition.Value.x,wellPosition.Value.y);
                Entities.ForEach((Entity f, ref ProjectileComponent p, ref Translation projectilePosition, ref MovementComponent moveComp, ref AffectedByGravityComponent abg) =>
                {
                    Vector2 pos2 = new Vector2(projectilePosition.Value.x, projectilePosition.Value.y);
                        Vector2 gravityForceDirection = pos - pos2;
                        gravityForceDirection.Normalize();
                        moveComp.movementVector += gravityForceDirection/2;
                });
            });
    }
}

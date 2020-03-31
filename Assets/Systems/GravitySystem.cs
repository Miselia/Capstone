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
                Entities.ForEach((Entity f, ref ProjectileComponent p, ref Translation projectilePosition, ref MovementComponent moveComp) =>
                {
                    Vector2 pos2 = new Vector2(projectilePosition.Value.x, projectilePosition.Value.y);
                    if (Vector2.Distance(pos, pos2) <= rad) {
                        moveComp.movementVector = Vector2.MoveTowards(pos2, pos, str);
                     }
                });
            });
    }
}

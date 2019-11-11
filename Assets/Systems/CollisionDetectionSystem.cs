using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CollisionDetectionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref XformComponent xform, ref CollisionComponent collComp) =>
        {

        });
    }
}

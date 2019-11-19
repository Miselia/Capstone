using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MovementComponent moveComp, ref Translation translation) =>
        {
            translation.Value.y += moveComp.movementVector.y * Time.deltaTime;
            translation.Value.x += moveComp.movementVector.x * Time.deltaTime;
        });
    }
}

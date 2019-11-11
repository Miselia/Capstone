using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MovementComponent moveComp, ref XformComponent xform) =>
        {
            xform.Move(moveComp.movementVector * Time.deltaTime);
        });
    }
}

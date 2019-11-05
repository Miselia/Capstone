using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct MovementComponent : IComponentData
{
    public Vector2 movementVector; // direction and speed simultaneously
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct RotationComponent : IComponentData
{
    // Rotating with direction is done by default
    // If this is false, then it is implied that the entity rotates over time, as determined by the RotationSystem until this is changed
    public bool rotateWithMovementDirection;

    public RotationComponent(bool rotateWithDirection)
    {
        rotateWithMovementDirection = rotateWithDirection;
    }
}

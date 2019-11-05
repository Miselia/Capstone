using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct RotationComponent : IComponentData
{
    public Quaternion movementVector; // Rotation of the entity
}

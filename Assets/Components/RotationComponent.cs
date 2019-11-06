using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct RotationComponent : IComponentData
{
    public Quaternion rotation; // Rotation of the entity

    public RotationComponent(Quaternion rotation)
    {
        this.rotation = rotation;
    }
}

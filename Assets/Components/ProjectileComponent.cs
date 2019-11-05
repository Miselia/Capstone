using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ProjectileComponent : IComponentData
{
    public string projectileName; // ID of the projectile
}

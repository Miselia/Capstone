using Unity.Entities;
using UnityEngine;

public struct ViperCurseComponent : IComponentData
{
    public float interval;
    public int bullets;
    public float maxInterval;

    public ViperCurseComponent(float interval)
    {
        this.interval = interval;
        this.maxInterval = interval;
        this.bullets = 4;
    }
}

using Unity.Entities;
using UnityEngine;

public struct ViperCurseComponent : IComponentData
{
    public float timer;
    public float maxTimer;

    public ViperCurseComponent(float timer)
    {
        this.timer = timer;
        this.maxTimer = timer;
    }
}

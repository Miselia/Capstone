using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ValueIncreaseComp : IComponentData
{

    public bool flag;
    public ValueIncreaseComp(bool flag)
    {
        this.flag = flag;
    }
}
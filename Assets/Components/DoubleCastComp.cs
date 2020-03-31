using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct DoubleCastComp : IComponentData
{

    public bool flag;
    public DoubleCastComp(bool flag)
    {
        this.flag = flag;
    }
}
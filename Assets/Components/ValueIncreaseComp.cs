using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ValueIncreaseComp : IComponentData
{

    //public bool flag;
    public int multiplier;

    public ValueIncreaseComp(/*bool flag, */int multi)
    {
        //this.flag = flag;
        multiplier = multi;
    }
}
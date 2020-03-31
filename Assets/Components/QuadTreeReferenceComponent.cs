using Assets.MonoScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

public struct QuadTreeReferenceComponent : IComponentData
{
    public int parentID;
    
    public QuadTreeReferenceComponent(int parentID)
    {
        this.parentID = parentID;
    }
}

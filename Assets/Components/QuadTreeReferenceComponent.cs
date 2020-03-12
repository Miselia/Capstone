using Assets.MonoScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace Assets.Components
{
    public class QuadTreeReferenceComponent : IComponentData
    {
        public QuadTreeNode node;
        
        public QuadTreeReferenceComponent(QuadTreeNode n)
        {
            node = n;
        }
    }
}

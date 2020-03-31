using Assets.MonoScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace Assets.Systems
{
    public class QuadTreeDrawingSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            foreach(QuadTreeNode qtn in QuadTreeSystem.quadTreeDict.Values)
            {
                CenteredRectangle cr = qtn.centeredRect;
                Debug.DrawLine(cr.topLeft, cr.topLeft + new Vector3(1, 0) * cr.width, Color.cyan);
                Debug.DrawLine(cr.topLeft, cr.topLeft + new Vector3(0, -1) * cr.height, Color.cyan);

                Debug.DrawLine(cr.botRight, cr.botRight + new Vector3(-1, 0) * cr.width, Color.cyan);
                Debug.DrawLine(cr.botRight, cr.botRight + new Vector3(0, 1) * cr.height, Color.cyan);
            }
        }
    }
}

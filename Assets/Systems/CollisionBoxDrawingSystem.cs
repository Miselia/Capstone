using Assets.MonoScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Systems
{
    public class CollisionBoxDrawingSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity firstEntity, ref CollisionComponent coll, ref Translation xform) =>
            {
                CenteredRectangle cr = new CenteredRectangle(coll);
            });
        }
    }
}

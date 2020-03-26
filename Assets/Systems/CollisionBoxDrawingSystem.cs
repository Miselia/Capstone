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
                CollisionComponent collComp = EntityManager.GetComponentData<CollisionComponent>(firstEntity);

                CenteredRectangle cr = new CenteredRectangle(collComp.width, collComp.collisionRadius, EntityManager.GetComponentData<Translation>(firstEntity).Value);

                Debug.DrawLine(cr.topLeft, cr.topLeft + new Vector3(1, 0) * cr.width, Color.red);
                Debug.DrawLine(cr.topLeft, cr.topLeft + new Vector3(0, -1) * cr.height, Color.red);

                Debug.DrawLine(cr.botRight, cr.botRight + new Vector3(-1, 0) * cr.width, Color.red);
                Debug.DrawLine(cr.botRight, cr.botRight + new Vector3(0, 1) * cr.height, Color.red);
            });
        }
    }
}

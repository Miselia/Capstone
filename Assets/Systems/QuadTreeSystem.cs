using Assets.MonoScript;
using Assets.Resources;
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
    public class QuadTreeSystem : ComponentSystem
    {
        public QuadTreeNode rootNode;
        public static Dictionary<int, QuadTreeNode> quadTreeDict;

        protected override void OnUpdate()
        {
            int counter = 0;
            quadTreeDict.Clear();
            rootNode = new QuadTreeNode(new CenteredRectangle(4 * Constants.GameBoundaryOffset, 2 * Constants.GameBoundaryOffset, new Unity.Mathematics.float3()));
            Entities.ForEach((Entity e, ref CollisionComponent coll, ref Translation translation) =>
            {
                counter++;
                rootNode.AddReference(e, coll, translation);
            });
            Debug.Log("Number of items added to tree = " + counter);
        }

        protected override void OnCreate()
        {
            quadTreeDict = new Dictionary<int, QuadTreeNode>();
            base.OnCreate();
        }
    }
}

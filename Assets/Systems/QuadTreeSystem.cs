using Assets.MonoScript;
using Assets.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Systems
{
    public class QuadTreeSystem : ComponentSystem
    {
        private QuadTreeNode rootNode;
        public static Dictionary<int, QuadTreeNode> quadTreeDict;

        protected override void OnUpdate()
        {
            rootNode = new QuadTreeNode(new CenteredRectangle(3 * Constants.GameBoundaryOffset, 3 * Constants.GameBoundaryOffset, new Unity.Mathematics.float3()));
            Entities.ForEach((Entity e, ref CollisionComponent coll, ref Translation translation) =>
            {
                rootNode.AddReference(e, coll, translation);
            });
        }

        protected override void OnCreate()
        {
            quadTreeDict = new Dictionary<int, QuadTreeNode>();
            base.OnCreate();
        }
    }
}

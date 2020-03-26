using Assets.Resources;
using Assets.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.MonoScript
{
    /*public struct QuadNode
    {
        public QuadTreeNode qtn;
    }*/

    public class QuadTreeNode
    {
        public int quadTreeRootID;
        public QuadTreeNode parent;
        public CenteredRectangle centeredRect;

        public bool allNodes = false;
        public List<Entity> leaves;
        public int maxLeavesBeforeSubTrees;
        public List<QuadTreeNode> subNodes;

        public QuadTreeNode(CenteredRectangle bounds)
        {
            quadTreeRootID = 0;
            parent = null;
            centeredRect = bounds;
            leaves = new List<Entity>();
            maxLeavesBeforeSubTrees = Constants.QuadTreeMaxReferences;
            subNodes = new List<QuadTreeNode>(4);
            QuadTreeSystem.quadTreeDict.Add(0, this);
            //QuadTreeJobSystem.quadTreeHashMap.Add(quadTreeRootID, new QuadNode { qtn = this });
        }
        public QuadTreeNode(CenteredRectangle bounds, QuadTreeNode parent, int leafNumber)
        {
            quadTreeRootID = 4 * parent.quadTreeRootID + leafNumber;
            this.parent = parent;
            centeredRect = bounds;
            leaves = new List<Entity>();
            maxLeavesBeforeSubTrees = Constants.QuadTreeMaxReferences;
            subNodes = new List<QuadTreeNode>(4);
            QuadTreeSystem.quadTreeDict.Add(quadTreeRootID, this);
            //QuadTreeJobSystem.quadTreeHashMap.Add(quadTreeRootID, new QuadNode { qtn = this });
        }

        public void AddReference(Entity entity, CollisionComponent coll, Translation translation)
        {
            if (!allNodes && leaves.Count < maxLeavesBeforeSubTrees)
            {
                leaves.Add(entity);
                World.Active.EntityManager.SetComponentData(entity, new QuadTreeReferenceComponent(quadTreeRootID));
            }
            else
            {
                if (!allNodes)
                {
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x - centeredRect.width / 4, centeredRect.center.y - centeredRect.height / 4, 0)),
                                                                        this, 1));
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x + centeredRect.width / 4, centeredRect.center.y - centeredRect.height / 4, 0)),
                                                                        this, 2));
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x - centeredRect.width / 4, centeredRect.center.y + centeredRect.height / 4, 0)),
                                                                        this, 3));
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x + centeredRect.width / 4, centeredRect.center.y + centeredRect.height / 4, 0)),
                                                                        this, 4));
                    allNodes = true;

                    List<Entity> temp = new List<Entity>();
                    temp.AddRange(leaves);
                    leaves.Clear();
                    AddReference(entity, coll, translation);
                    foreach (Entity e in temp)
                    {
                        AddReference(e, World.Active.EntityManager.GetComponentData<CollisionComponent>(e), World.Active.EntityManager.GetComponentData<Translation>(e));
                    }
                }
                else
                {
                    bool noContains = true;
                    foreach (QuadTreeNode qtn in subNodes)
                    {
                        CenteredRectangle entityBounds = new CenteredRectangle(coll.width, coll.collisionRadius, translation.Value);
                        if(qtn.centeredRect.Contains(entityBounds))
                        {
                            qtn.AddReference(entity, coll, translation);
                            noContains = false;
                            break;
                        }
                    }
                    if (noContains)
                    {
                        leaves.Add(entity);
                        World.Active.EntityManager.SetComponentData(entity, new QuadTreeReferenceComponent(quadTreeRootID));
                    }
                }
            }
        }
    }
}

using Assets.Components;
using Assets.Resources;
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
    public class QuadTreeNode
    {
        public QuadTreeNode parent;
        public CenteredRectangle centeredRect;

        public bool allNodes = false;
        public List<Entity> leaves;
        public int maxLeavesBeforeSubTrees;
        public List<QuadTreeNode> subNodes;

        public QuadTreeNode(CenteredRectangle bounds)
        {
            parent = null;
            centeredRect = bounds;
            leaves = new List<Entity>();
            maxLeavesBeforeSubTrees = Constants.QuadTreeMaxReferences;
            subNodes = new List<QuadTreeNode>(4);
        }
        public QuadTreeNode(CenteredRectangle bounds, QuadTreeNode parent)
        {
            this.parent = parent;
            centeredRect = bounds;
            leaves = new List<Entity>();
            maxLeavesBeforeSubTrees = Constants.QuadTreeMaxReferences;
            subNodes = new List<QuadTreeNode>(4);
        }

        public void AddReference(Entity entity, CollisionComponent coll, Translation translation)
        {
            if (!allNodes && leaves.Count < maxLeavesBeforeSubTrees)
            {
                leaves.Add(entity);
            }
            else
            {
                if (!allNodes)
                {
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x - centeredRect.width/2, centeredRect.center.y - centeredRect.height/2, 0)),
                                                                        this));
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x + centeredRect.width / 2, centeredRect.center.y - centeredRect.height / 2, 0)),
                                                                        this));
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x - centeredRect.width / 2, centeredRect.center.y + centeredRect.height / 2, 0)),
                                                                        this));
                    subNodes.Add(new QuadTreeNode(new CenteredRectangle(centeredRect.width / 2, centeredRect.height / 2,
                                                                        new float3(centeredRect.center.x + centeredRect.width / 2, centeredRect.center.y + centeredRect.height / 2, 0)),
                                                                        this));
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
                    /*Translation translation = World.Active.EntityManager.GetComponentData<Translation>(entity);
                    CollisionComponent coll = World.Active.EntityManager.GetComponentData<CollisionComponent>(entity);*/

                    bool noContains = true;
                    foreach (QuadTreeNode qtn in subNodes)
                    {
                        /*CenteredRectangle entityAABB = World.Active.EntityManager.GetComponentData<CollisionComponent>(entity)
                                                        .GetAABB((float)Math.Cos(xfrom.Rotation),
                                                        (float)Math.Sin(xfrom.Rotation),
                                                        xfrom.Scale);
                        entityAABB.Min += xfrom.Position;
                        entityAABB.Max += xfrom.Position;
                        if (qtn.centeredRect.Contains(entityAABB))
                        {
                            qtn.AddReference(entity);
                            noIntersect = false;
                            break;
                        }*/
                        CenteredRectangle entityBounds = new CenteredRectangle(coll.collisionRadius, coll.collisionRadius, translation.Value);
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
                        World.Active.EntityManager.AddComponentObject(entity, new QuadTreeReferenceComponent(this));
                    }
                }
            }
        }
    }
}

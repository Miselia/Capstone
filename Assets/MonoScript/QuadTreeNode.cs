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

        public void AddReference(Entity entity)
        {
            if (!allNodes && leaves.Count < maxLeavesBeforeSubTrees)
            {
                leaves.Add(entity);
                /*World.Active.EntityManager.SetComponentData<QuadTreeReferenceComponent>(entity, new QuadTreeReferenceComponent(this));
                World.Active.EntityManager.AddComponentData<QuadTreeReferenceComponent>(entity, new QuadTreeReferenceComponent(this));*/
                World.Active.EntityManager.AddComponentObject(entity, new QuadTreeReferenceComponent(this));
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
                    AddReference(entity);
                    foreach (Entity e in temp)
                    {
                        AddReference(e);
                    }
                }
                else
                {
                    Translation translation = World.Active.EntityManager.GetComponentData<Translation>(entity);
                    CollisionComponent coll = World.Active.EntityManager.GetComponentData<CollisionComponent>(entity);

                    bool noIntersect = true;
                    foreach (QuadTreeNode qtn in subNodes)
                    {
                        CenteredRectangle entityBounds = new CenteredRectangle(coll.collisionRadius, coll.collisionRadius, translation.Value);
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
                    }
                    if (noIntersect)
                    {
                        leaves.Add(entity);
                        World.Active.EntityManager.AddComponentObject(entity, new QuadTreeReferenceComponent(this));
                    }
                }
            }
        }
    }
}

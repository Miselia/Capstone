using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Assets.Resources;

public static class PlayerBoundaryEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 normal, Mesh mesh, Material mat, int side)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(PlayerBoundaryComponent));
        em.AddComponent(entity, typeof(Translation));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Scale));
        em.AddComponent(entity, typeof(QuadTreeReferenceComponent));
        em.AddComponent(entity, typeof(IsBoundaryComponent));

        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(0f, 1000, 0x0c));
        em.SetComponentData(entity, new PlayerBoundaryComponent(normal, side));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });
        em.SetComponentData(entity, new QuadTreeReferenceComponent(-1));

        // For now the scale is clost to the actual boundaries, for now simply change the value
        /* Currently tested values:
         *  Size = 13, - .850f
         *  Size = 10, + .125f
         *  */
        em.SetComponentData(entity, new Scale { Value = Constants.PlayerBoundarySize - .850f });

        return entity;
    }
}

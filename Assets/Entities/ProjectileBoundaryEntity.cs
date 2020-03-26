using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class ProjectileBoundaryEntity
{
    public static Entity Create(EntityManager em, Vector2 position, Vector2 normal,
                                Mesh mesh, Material mat, float scale, Color tint)
    {
        Entity entity = em.CreateEntity();

        em.AddComponent(entity, typeof(XformComponent));
        em.AddComponent(entity, typeof(CollisionComponent));
        em.AddComponent(entity, typeof(ProjectileBoundaryComponent));
        em.AddComponent(entity, typeof(Translation));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Scale));
        em.AddComponent(entity, typeof(QuadTreeReferenceComponent));
        em.AddComponent(entity, typeof(IsBoundaryComponent));

        em.SetComponentData(entity, new XformComponent(position));
        em.SetComponentData(entity, new CollisionComponent(0f, 1000, 0x02));
        em.SetComponentData(entity, new ProjectileBoundaryComponent(normal));
        em.SetComponentData(entity, new Translation { Value = new float3(position.x, position.y, 0) });
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });
        em.SetComponentData(entity, new Scale { Value = scale });
        em.SetComponentData(entity, new QuadTreeReferenceComponent(-1));
        mat.color = tint;

        return entity;
    }
}

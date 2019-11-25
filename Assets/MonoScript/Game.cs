using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Game : MonoBehaviour
{
    private EntityManager entityManager;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    [SerializeField] private Material projectileMat;
    [SerializeField] private Material vertBoundaryMat;
    [SerializeField] private Material horiBoundaryMat;
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.Active.EntityManager;
        PlayerEntity.Create(entityManager, new Vector2(0,5), new Vector2(0, -1), 1.0f, 1, 0, mesh,mat);
        ProjectileEntity.Create(entityManager, new Vector2(0,-5), new Vector2(0, 1), 1.0f, mesh, projectileMat);

        PlayerEntity.Create(entityManager, new Vector2(5, 0), new Vector2(1, -1), 1.0f, 1, 1, mesh, mat);
        ProjectileEntity.Create(entityManager, new Vector2(-5, 0), new Vector2(-1, 1), 1.0f, mesh, projectileMat);

        BoundaryEntity.Create(entityManager, new Vector2(0, 7), new Vector2(0, -1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(0, -7), new Vector2(0, 1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(7, 0), new Vector2(-1, 0), mesh, vertBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(-7, 0), new Vector2(1, 0), mesh, vertBoundaryMat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public EntityManager getEntityManager()
    {
        return entityManager;
    }
}

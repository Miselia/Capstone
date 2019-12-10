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
    [SerializeField] private Material cardMat;
    
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.Active.EntityManager;
        
        PlayerEntity.Create(entityManager, new Vector2(0,5), new Vector2(0, 0), 1.0f, 1, 0, mesh,mat);

        BoundaryEntity.Create(entityManager, new Vector2(0, 7), new Vector2(0, -1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(0, -7), new Vector2(0, 1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(7, 0), new Vector2(-1, 0), mesh, vertBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(-7, 0), new Vector2(1, 0), mesh, vertBoundaryMat);

        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 1, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 2, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 3, 1, mesh, cardMat);
        getSpawner().spawn(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public EntityManager getEntityManager()
    {
        return entityManager;
    }
    public Spawner getSpawner()
    {
        Spawner spawner = new Spawner(entityManager, mesh, projectileMat);
        return spawner;
    }
}

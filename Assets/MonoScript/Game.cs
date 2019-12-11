using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Game : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    [SerializeField] private Material projectileMat;
    [SerializeField] private Material vertBoundaryMat;
    [SerializeField] private Material horiBoundaryMat;
    [SerializeField] private Material cardMat;
    
    // Start is called before the first frame update
    private EntityManager entityManager;
    private Spawner spawner;
    [SerializeField] public EventManager eventManager;
    void Start()
    {
        eventManager = gameObject.AddComponent<EventManager>();
        entityManager = World.Active.EntityManager;
        spawner = new Spawner(entityManager, mesh, projectileMat);
        PlayerEntity.Create(entityManager, new Vector2(0,5), new Vector2(0, 0), 1.0f, 1, 0, mesh,mat);

        BoundaryEntity.Create(entityManager, new Vector2(0, 7), new Vector2(0, -1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(0, -7), new Vector2(0, 1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(7, 0), new Vector2(-1, 0), mesh, vertBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(-7, 0), new Vector2(1, 0), mesh, vertBoundaryMat);

        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 1, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 2, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 3, 1, mesh, cardMat);
        spawner.spawn(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        eventManager.EmptyQueue();
    }

    public EntityManager getEntityManager()
    {
        return entityManager;
    }
    public Spawner getSpawner()
    {
        return spawner;
    }
    
}

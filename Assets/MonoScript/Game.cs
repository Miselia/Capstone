using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

public class Game : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    [SerializeField] private Material vertBoundaryMat;
    [SerializeField] private Material horiBoundaryMat;
    [SerializeField] private Material cardMat;

    public Dictionary<Entity, List<Entity>> collidingPairs = new Dictionary<Entity, List<Entity>>();

    // Start is called before the first frame update
    private EntityManager entityManager;
    private Spawner spawner;
    [SerializeField] public EventManager eventManager;
    void Start()
    {
        eventManager = gameObject.AddComponent<EventManager>();
        EventManager.instance.RegisterListener<EndCollisionEvent>(this);

        entityManager = World.Active.EntityManager;
        spawner = gameObject.AddComponent<Spawner>();
        PlayerEntity.Create(entityManager, new Vector2(0,5), new Vector2(0, 0), 0.5f, 1, 0, mesh,mat);

        BoundaryEntity.Create(entityManager, new Vector2(0, 7), new Vector2(0, -1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(0, -7), new Vector2(0, 1), mesh, horiBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(7, 0), new Vector2(-1, 0), mesh, vertBoundaryMat);
        BoundaryEntity.Create(entityManager, new Vector2(-7, 0), new Vector2(1, 0), mesh, vertBoundaryMat);

        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 1, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 2, 2, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 3, 3, 1, mesh, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 4, 4, 1, mesh, cardMat);
    }

    public EntityManager getEntityManager()
    {
        return entityManager;
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if(evt is EndCollisionEvent)
        {
            EndCollisionEvent ece = evt as EndCollisionEvent;
            HandleEndCollisionEvent(ece.entityA, ece.entityB);
            return true;
        }
        return false;
    }

    private void HandleEndCollisionEvent(Entity entityA, Entity entityB)
    {
        if (collidingPairs[entityA].Contains(entityB))
            collidingPairs[entityA].Remove(entityB);
        if (collidingPairs[entityB].Contains(entityA))
            collidingPairs[entityB].Remove(entityA);
    }
}

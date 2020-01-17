using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Entities;

public class Game : MonoBehaviour, IGenericEventListener
{
    private int boundaryOffest = 10;
    [SerializeField] private Mesh mesh2D;
    [SerializeField] private Material playerMat;
    [SerializeField] private Material vertBoundaryMat;
    [SerializeField] private Material horiBoundaryMat;
    [SerializeField] private Material cardMat;

    public Dictionary<Entity, List<Entity>> collidingPairs = new Dictionary<Entity, List<Entity>>();

    // Start is called before the first frame update
    private EntityManager entityManager;
    private Spawner spawner;
    private Deck playDeck1;
    private Deck playDeck2;
    [SerializeField] public EventManager eventManager;
    void Start()
    {
        eventManager = gameObject.AddComponent<EventManager>();
        EventManager.instance.RegisterListener<EndCollisionEvent>(this);

        entityManager = World.Active.EntityManager;
        spawner = gameObject.AddComponent<Spawner>();
        PlayerEntity.Create(entityManager, new Vector2(-10,0), new Vector2(0, 0), 0.5f, 1, 3, mesh2D, playerMat);
        PlayerEntity.Create(entityManager, new Vector2(10, 0), new Vector2(0, 0), 0.5f, 2, 3, mesh2D, playerMat);

        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffest+boundaryOffest/2, 0), new Vector2(-1, 0), mesh2D, vertBoundaryMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffest/2, 0), new Vector2(1, 0), mesh2D, vertBoundaryMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffest, -boundaryOffest/2), new Vector2(0, 1), mesh2D, horiBoundaryMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffest, boundaryOffest/2), new Vector2(0, -1), mesh2D, horiBoundaryMat);

        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffest-boundaryOffest/2, 0), new Vector2(1, 0), mesh2D, vertBoundaryMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffest/2, 0), new Vector2(-1, 0), mesh2D, vertBoundaryMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffest, -boundaryOffest/2), new Vector2(0, 1), mesh2D, horiBoundaryMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffest, boundaryOffest/2), new Vector2(0, -1), mesh2D, horiBoundaryMat);

        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2 * -boundaryOffest, 0), new Vector2(1,0), mesh2D, vertBoundaryMat, 20.0f);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2 * boundaryOffest, 0), new Vector2(-1, 0), mesh2D, vertBoundaryMat, 20.0f);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, boundaryOffest), new Vector2(0, -1), mesh2D, horiBoundaryMat, 40.3f);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, -boundaryOffest), new Vector2(0, 1), mesh2D, horiBoundaryMat, 40.3f);

        CardEntity.Create(entityManager, new Vector2(-10, -9), 1, 1, 1, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 2, 2, 1, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 3, 3, 1, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(-10, -9), 4, 4, 1, mesh2D, cardMat);

        playDeck1 = new Deck("player1.txt");
        for(int i = 1; i <= 4; i++)
        {
            CardEntity.Create(entityManager, new Vector2(-10, -9), playDeck1.drawCard(), i, 1, mesh2D, cardMat);
        }
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

using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Entities;
using Assets.Resources;
using Assets.MonoScript;

public class DeckBuilderGame : MonoBehaviour, IGame
{
    private int boundaryOffset;
    private int boundarySize;
    private int maxHealth;
    private float maxMana;
    private float manaRegen;
    private float playerRadius;
    private Dictionary<Entity, List<Entity>> collidingPairs;

    private EntityManager entityManager;
    private Spawner spawner;

    public EventManager eventManager;
    [SerializeField] private Mesh mesh2D;
    [SerializeField] private Material playerMat;
    [SerializeField] private Material vertPlayerBoundMat;
    [SerializeField] private Material horiPlayerBoundMat;
    [SerializeField] private Material vertProjectileBoundMat;
    [SerializeField] private Material horiProjectileBoundMat;

    void Start()
    {
        eventManager = gameObject.AddComponent<EventManager>();
        EventManager.instance.RegisterListener<EndCollisionEvent>(this);

        entityManager = World.Active.EntityManager;
        spawner = gameObject.AddComponent<Spawner>();

        collidingPairs = new Dictionary<Entity, List<Entity>>();

        boundaryOffset = Constants.GameBoundaryOffset;
        boundarySize = Constants.PlayerBoundarySize;

        maxHealth = Constants.PlayerMaximumHealth;
        maxMana = Constants.PlayerMaximumMana;
        manaRegen = Constants.PlayerManaRegen;
        playerRadius = Constants.PlayerRadius;

        PlayerEntity.Create(entityManager, new Vector2(boundaryOffset, 0), new Vector2(0, 0), playerRadius, 1, maxHealth, maxMana, manaRegen, mesh2D, playerMat);

        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 1));
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 2));

        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset + boundarySize / 2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset - boundarySize / 2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundarySize / 2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundarySize / 2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat);

        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, 0), new Vector2(1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.red);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2*boundaryOffset, 0), new Vector2(-1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.red);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundaryOffset), new Vector2(0, -1), mesh2D, horiProjectileBoundMat, 20.0f, Color.red);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundaryOffset), new Vector2(0, 1), mesh2D, horiProjectileBoundMat, 20.0f, Color.red);
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is EndCollisionEvent)
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

    public Dictionary<Entity, List<Entity>> GetCollidingPairs()
    {
        return collidingPairs;
    }
}

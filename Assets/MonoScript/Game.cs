﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Entities;
using Assets.Resources;

public class Game : MonoBehaviour, IGenericEventListener
{
    // boundaryOffset represents how far the center of a player boundary is from the center of the screen
    private int boundaryOffset = Constants.PlayerBoundaryOffset;
    private int boundarySize = Constants.PlayerBoundarySize;
    private int maxHealth = Constants.PlayerMaximumHealth;
    private float maxMana = Constants.PlayerMaximumMana;
    private float manaRegen = Constants.PlayerManaRegen;
    private float playerRadius = Constants.PlayerRadius;

    [SerializeField] private Mesh mesh2D;
    [SerializeField] private Material playerMat;
    [SerializeField] private Material vertPlayerBoundMat;
    [SerializeField] private Material horiPlayerBoundMat;
    [SerializeField] private Material vertProjectileBoundMat;
    [SerializeField] private Material horiProjectileBoundMat;
    [SerializeField] private List<Material> cardMaterialLibrary;

    public Dictionary<Entity, List<Entity>> collidingPairs = new Dictionary<Entity, List<Entity>>();

    // Start is called before the first frame update
    private EntityManager entityManager;
    private Deck playDeck1;
    private Deck playDeck2;
    [SerializeField] public EventManager eventManager;
    [SerializeField] public Spawner spawner;
    void Start()
    {
        eventManager = gameObject.AddComponent<EventManager>();
        EventManager.instance.RegisterListener<EndCollisionEvent>(this);

        entityManager = World.Active.EntityManager;
        spawner = gameObject.AddComponent<Spawner>();

        PlayerEntity.Create(entityManager, new Vector2(-boundaryOffset,0), new Vector2(0, 0), playerRadius, 1, maxHealth, maxMana, manaRegen, mesh2D, playerMat);
        PlayerEntity.Create(entityManager, new Vector2(boundaryOffset, 0), new Vector2(0, 0), playerRadius, 2, maxHealth, maxMana, manaRegen, mesh2D, playerMat);
        /*
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, maxMana, 2));
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, maxMana, 1));
        */
        playDeck1 = new Deck("player1.txt");
        playDeck2 = new Deck("player2.txt");

        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset+boundarySize/2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset-boundarySize/2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundarySize/2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundarySize/2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat);

        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset-boundarySize/2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset+boundarySize/2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset, -boundarySize/2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset, boundarySize/2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat);

        /*
        CardEntity.Create(entityManager, new Vector2(-boundaryOffset-7, -7.5f), 1, 1, 1, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(-boundaryOffset-7, -7.5f), 2, 2, 1, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(-boundaryOffset-7, -7.5f), 3, 3, 1, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(-boundaryOffset-7, -7.5f), 4, 4, 1, mesh2D, cardMat);

        CardEntity.Create(entityManager, new Vector2(boundaryOffset-7, -7.5f), 1, 1, 2, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(boundaryOffset-7, -7.5f), 2, 2, 2, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(boundaryOffset-7, -7.5f), 3, 3, 2, mesh2D, cardMat);
        CardEntity.Create(entityManager, new Vector2(boundaryOffset-7, -7.5f), 4, 4, 2, mesh2D, cardMat);
        */

        ProjectileBoundaryEntity.Create(entityManager, new Vector2(-2 * boundaryOffset, 0), new Vector2(1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.red);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2 * boundaryOffset, 0), new Vector2(-1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.red);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, boundaryOffset), new Vector2(0, -1), mesh2D, horiProjectileBoundMat, 40.3f, Color.red);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, -boundaryOffset), new Vector2(0, 1), mesh2D, horiProjectileBoundMat, 40.3f, Color.red);
    }

    /*
    public EntityManager getEntityManager()
    {
        return entityManager;
    }
    */

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

    public int drawCardFromDeck(int player, int cardSlot)
    {
       if(player == 1)
        {
            int nextCard = playDeck1.drawCard();
            if (nextCard != 0) CardEntity.Create(entityManager, new Vector2(-boundaryOffset - 7.5f, -7.5f), nextCard, cardSlot, player, mesh2D, cardMaterialLibrary[nextCard]);
            //else playDeck1.Shuffle();
            return nextCard;
        }
       else
       {
            int nextCard = playDeck2.drawCard();
            if (nextCard != 0) CardEntity.Create(entityManager, new Vector2(boundaryOffset - 7.5f, -7.5f), nextCard, cardSlot, player, mesh2D, cardMaterialLibrary[nextCard]);
            //else playDeck2.Shuffle();
            return nextCard;
        }
    }
    public void reshuffle(int player)
    {
        if (player == 1)
        {
            playDeck1.Shuffle();
        }
        else
        {
            playDeck2.Shuffle();
        }
    }


    private void HandleEndCollisionEvent(Entity entityA, Entity entityB)
    {
        if (collidingPairs[entityA].Contains(entityB))
            collidingPairs[entityA].Remove(entityB);
        if (collidingPairs[entityB].Contains(entityA))
            collidingPairs[entityB].Remove(entityA);
    }
    
}

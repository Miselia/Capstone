﻿using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Entities;
using Assets.Resources;
using Assets.MonoScript;
using Assets.Events.GenericEvents;

public class DeckBuilderGame : MonoBehaviour, IGame
{
    private int boundaryOffset = Constants.DeckBuilderBoundaryOffset;
    private int boundarySize = Constants.PlayerBoundarySize;
    private int maxHealth = Constants.PlayerMaximumHealth;
    private float maxMana = Constants.PlayerMaximumMana;
    private float manaRegen = Constants.PlayerManaRegen;
    private float playerRadius = Constants.PlayerRadius;
    private Dictionary<Entity, List<Entity>> collidingPairs = new Dictionary<Entity, List<Entity>>();

    private EntityManager entityManager;
    private Spawner spawner;

    [SerializeField] public EventManager eventManager;
    [SerializeField] public Mesh mesh2D;
    [SerializeField] private Material playerMat;
    [SerializeField] private Material vertPlayerBoundMat;
    [SerializeField] private Material horiPlayerBoundMat;
    [SerializeField] private Material vertProjectileBoundMat;
    [SerializeField] private Material horiProjectileBoundMat;
    [SerializeField] private CardLibrary cl;

    int[] playerHand;

    private List<CardData> cardLibrary;
    public Deck builderDeck;


    void Start()
    {
        Debug.Log(typeof(ButtonListController).ToString());
        playerHand = new int[] { -1, -1, -1, -1 };
        GetDeck();
        Debug.Log("Builder deck is null: " + (builderDeck == null));

        eventManager = gameObject.AddComponent<EventManager>();
        EventManager.instance.RegisterListener<EndCollisionEvent>(this);
        EventManager.instance.RegisterListener<AddCardtoDeckEvent>(this);
        EventManager.instance.RegisterListener<DeckBuilderHandAdjustEvent>(this);

        entityManager = World.Active.EntityManager;
        spawner = gameObject.AddComponent<Spawner>();

        cardLibrary = cl.GetListByID(builderDeck.getFactions()[0]);
        cardLibrary.AddRange(cl.GetListByID(builderDeck.getFactions()[1]));

        World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = true;
        World.Active.GetExistingSystem<ControlSystem>().Enabled = true;
        World.Active.GetExistingSystem<DeletionSystem>().Enabled = true;
        World.Active.GetExistingSystem<MovementSystem>().Enabled = true;
        World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = true;

        PlayerEntity.Create(entityManager, new Vector2(boundaryOffset, 0), new Vector2(0, 0), playerRadius, 1, maxHealth, maxMana, manaRegen, mesh2D, playerMat);

        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 1));
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 2));

        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset + boundarySize / 2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset - boundarySize / 2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundarySize / 2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundarySize / 2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat);

        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, 0), new Vector2(1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2*boundaryOffset, 0), new Vector2(-1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundaryOffset), new Vector2(0, -1), mesh2D, horiProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundaryOffset), new Vector2(0, 1), mesh2D, horiProjectileBoundMat, 20.0f, Color.clear);

        EventManager.instance.QueueEvent(new InitializeDeckBuilderListUIEvent());
        EventManager.instance.QueueEvent(new InitializeDeckBuilerDeckUIEvent());
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is EndCollisionEvent)
        {
            EndCollisionEvent ece = evt as EndCollisionEvent;
            HandleEndCollisionEvent(ece.entityA, ece.entityB);
            return true;
        }
        if (evt is AddCardtoDeckEvent)
        {
            Debug.Log("Add Card to Deck Event recieved");
            AddCardtoDeckEvent acd = evt as AddCardtoDeckEvent;
            bool result = AddCardToDeck(acd.cardID);
            if (result)
            {
                Debug.Log("Calling Add to Deck List UI");
                EventManager.instance.QueueEvent(new AddCardtoDeckScrollListEvent(acd.cardID, acd.cardName));
                return true;
            }
            else
            {
                Debug.Log("Add to Deck List failed, abort add to UI");
                return false;
            }
        }
        if (evt is DeckBuilderHandAdjustEvent)
        {
            DeckBuilderHandAdjustEvent se = evt as DeckBuilderHandAdjustEvent;
            AdjustPlayerHand(se.card);
            return true;
        }
        return false;
    }

    private void AdjustPlayerHand(Entity card)
    {
        // This code does not terminate early since the max number of checks is the max hand size of 4
        for(int i=0; i<playerHand.Length; i++)
        {
            if(playerHand[i] == entityManager.GetComponentData<CardComp>(card).cardID)
            {
                playerHand[i] = -1;
                Debug.Log("Player Hand [" + i + "] changed to -1 from spawn call");
            }
            else
            {
                Debug.Log("Player Hand [" + i + "] not adjusted from spawn call");
            }
        }
    }

    private void HandleEndCollisionEvent(Entity entityA, Entity entityB)
    {
        if (collidingPairs[entityA].Contains(entityB))
            collidingPairs[entityA].Remove(entityB);
        if (collidingPairs[entityB].Contains(entityA))
            collidingPairs[entityB].Remove(entityA);
    }

    public void GetDeck()
    {
        builderDeck = DeckLobbyScript.chosenDeck;
    }

    public Dictionary<Entity, List<Entity>> GetCollidingPairs()
    {
        return collidingPairs;
    }

    public bool AddCardtoHand(int cardID)
    {
        int nextSlot = FindOpenHandSlot(cardID);
        if (nextSlot != -1)
        {
            CardEntity.Create(entityManager, new Vector2(boundaryOffset - 7, -7.5f), cardID, nextSlot, 1, cl.GetAllByID()[cardID].manaCost, mesh2D, cl.GetAllByID()[cardID].getMaterial());
            return true;
        }
        else
            return false;
    }

    private bool AddCardToDeck(int cardID)
    {
        return builderDeck.AddCard(cardID);
    }

    public bool RemoveCardFromDeck(int cardID)
    {
        return builderDeck.RemoveCard(cardID);
    }

    public List<CardData> GetCardLibrary()
    {
        return cardLibrary;
    }

    private int FindOpenHandSlot(int id)
    {
        for(int i=0; i<playerHand.Length; i++)
        {
            if(playerHand[i] == -1)
            {
                playerHand[i] = id;
                return i+1;
            }
        }
        return -1;
    }
}

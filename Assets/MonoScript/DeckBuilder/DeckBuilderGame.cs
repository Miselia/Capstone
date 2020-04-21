using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using System;
using Assets.Resources;
using Assets.MonoScript;
using Assets.Events.GenericEvents;
using UnityEngine.SceneManagement;
using Assets.Systems;

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
    [SerializeField] private Image CardMenu;
    [SerializeField] private Image DeckMenu;

    int[] playerHand;

    private List<CardData> cardLibrary;
    public Deck builderDeck;


    void Start()
    {
        Debug.Log(typeof(ButtonListController).ToString());
        playerHand = new int[] { -1, -1, -1, -1 };
        GetDeck();
        //Debug.Log("Builder deck is null: " + (builderDeck == null));

        eventManager = gameObject.AddComponent<EventManager>();
        EventManager.instance.RegisterListener<EndCollisionEvent>(this);
        EventManager.instance.RegisterListener<AddCardtoDeckEvent>(this);
        EventManager.instance.RegisterListener<DeckBuilderHandAdjustEvent>(this);

        entityManager = World.Active.EntityManager;
        spawner = gameObject.AddComponent<Spawner>();

        cardLibrary = cl.GetListByID(builderDeck.getFactions()[0]);
        cardLibrary.AddRange(cl.GetListByID(builderDeck.getFactions()[1]));

        World.Active.GetExistingSystem<QuadTreeSystem>().Enabled = true;
        World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = true;
        World.Active.GetExistingSystem<ControlSystem>().Enabled = true;
        World.Active.GetExistingSystem<DeletionSystem>().Enabled = true;
        World.Active.GetExistingSystem<MovementSystem>().Enabled = true;
        World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = true;
        World.Active.GetExistingSystem<BuffSystem>().Enabled = true;
        World.Active.GetExistingSystem<SpawnDelaySystem>().Enabled = true;
        World.Active.GetExistingSystem<RotationSystem>().Enabled = true;
        World.Active.GetExistingSystem<GravitySystem>().Enabled = true;

        //World.Active.GetExistingSystem<CollisionBoxDrawingSystem>().Enabled = true;
        //World.Active.GetExistingSystem<QuadTreeDrawingSystem>().Enabled = true;
        
        PlayerEntity.Create(entityManager, new Vector2(boundaryOffset, 0), new Vector2(0, 0), playerRadius, 1, maxHealth, maxMana, manaRegen, mesh2D, playerMat);

        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 1));
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 2));

        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset + boundarySize / 2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat, 1);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset - boundarySize / 2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat, 1);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundarySize / 2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat, 1);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundarySize / 2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat, 1);

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
            //Debug.Log("Add Card to Deck Event recieved");
            AddCardtoDeckEvent acd = evt as AddCardtoDeckEvent;
            bool result = AddCardToDeck(acd.cardID);
            if (result)
            {
                //Debug.Log("Calling Add to Deck List UI");
                EventManager.instance.QueueEvent(new AddCardtoDeckScrollListEvent(acd.cardID, acd.cardName, acd.traits, acd. flavor));
                return true;
            }
            else
            {
                //Debug.Log("Add to Deck List failed, abort add to UI");
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
                //Debug.Log("Player Hand [" + i + "] changed to -1 from spawn call");
            }
            else
            {
                //Debug.Log("Player Hand [" + i + "] not adjusted from spawn call");
            }
        }
    }
    public void SaveAndExit()
    {
        builderDeck.SaveDeck();
        
        World.Active.GetExistingSystem<DrawSystem>().Enabled = false;
        World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = false;
        World.Active.GetExistingSystem<ControlSystem>().Enabled = false;
        World.Active.GetExistingSystem<DeletionSystem>().Enabled = false;
        World.Active.GetExistingSystem<MovementSystem>().Enabled = false;
        World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = false;
        foreach (Entity e in World.Active.EntityManager.GetAllEntities())
        {
            World.Active.EntityManager.DestroyEntity(e);
        }
        SceneManager.LoadScene("DeckBuilderLobby"); ;
        
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
        CardMenu.color = GetFactionColor(builderDeck.getFactions()[0]);
        DeckMenu.color = GetFactionColor(builderDeck.getFactions()[1]);
    }
    public Color GetFactionColor(string faction)
    {
        switch (faction)
        {
            case "Fantasy":
                return new Color(0.95f, 0.72f, 0.94f, 100);
                break;
            case "Steampunk":
                return new Color(0.68f, 0.39f, 0.16f, 100);
                break;
            case "Sci-Fi":
                return new Color(0.56f, 0.92f, 0.90f, 100);
                break;
            case "Horror":
                return new Color(0.68f, 0.01f, 0, 100);
                break;
                
        }
        return new Color(0, 0, 0);
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

    public int AddCardToHandFromCardLibrary(int player, int cardSlot, int cardID)
    {
        CardEntity.Create(entityManager, new Vector2(boundaryOffset - 7, -7.5f), cardID, cardSlot, 1, cl.GetAllByID()[cardID].manaCost, mesh2D, cl.GetAllByID()[cardID].getMaterial());
        return cardID;
    }
}

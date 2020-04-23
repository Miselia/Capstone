using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Resources;
using Assets.MonoScript;
using Assets.Systems;

public class Game : MonoBehaviour, IGame
{
    // boundaryOffset represents how far the center of a player boundary is from the center of the screen
    private int boundaryOffset = Constants.GameBoundaryOffset;
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
    [SerializeField] private CardLibrary cl;

    private List<CardData> cardLibrary;

    private Dictionary<Entity, List<Entity>> collidingPairs = new Dictionary<Entity, List<Entity>>();

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

        
        //Debug.Log(cardLibrary[0].cardName + "," + cardLibrary[1].cardName + "," + cardLibrary[2].cardName + "," + cardLibrary[3].cardName);
        World.Active.GetExistingSystem<DrawSystem>().Enabled = true;
        World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = true;
        World.Active.GetExistingSystem<ControlSystem>().Enabled = true;
        World.Active.GetExistingSystem<DeletionSystem>().Enabled = true;
        World.Active.GetExistingSystem<MovementSystem>().Enabled = true;
        World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = true;
        World.Active.GetExistingSystem<QuadTreeSystem>().Enabled = true;
        World.Active.GetExistingSystem<BuffSystem>().Enabled = true;
        World.Active.GetExistingSystem<SpawnDelaySystem>().Enabled = true;
        World.Active.GetExistingSystem<RotationSystem>().Enabled = true;
        World.Active.GetExistingSystem<GravitySystem>().Enabled = true;

        World.Active.GetExistingSystem<QuadTreeDrawingSystem>().Enabled = true;
        //World.Active.GetExistingSystem<CollisionBoxDrawingSystem>().Enabled = true;
        //World.Active.GetExistingSystem<CollisionBoxDrawingSystem>().Enabled = true;
        //World.Active.GetExistingSystem<QuadTreeJobSystem>().Enabled = true;
        //World.Active.GetExistingSystem<QuadTreeJobDrawingSystem>().Enabled = true;

        cardLibrary = cl.GetAllByID();

        playDeck1 = LobbyScript.p1Deck;
        playDeck2 = LobbyScript.p2Deck;

        PlayerEntity.Create(entityManager, new Vector2(-boundaryOffset,0), new Vector2(0, 0), playerRadius, 1, maxHealth, maxMana, manaRegen, mesh2D, playDeck1.GetPrimary(), playerMat);
        PlayerEntity.Create(entityManager, new Vector2(boundaryOffset, 0), new Vector2(0, 0), playerRadius, 2, maxHealth, maxMana, manaRegen, mesh2D, playDeck2.GetPrimary(), playerMat);
        
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 1));
        EventManager.instance.QueueEvent(new UIUpdateEvent(maxHealth, (int)maxMana, 2));

        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset+boundarySize/2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset-boundarySize/2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, -boundarySize/2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(boundaryOffset, boundarySize/2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat);

        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset-boundarySize/2, 0), new Vector2(1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset+boundarySize/2, 0), new Vector2(-1, 0), mesh2D, vertPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset, -boundarySize/2), new Vector2(0, 1), mesh2D, horiPlayerBoundMat);
        PlayerBoundaryEntity.Create(entityManager, new Vector2(-boundaryOffset, boundarySize/2), new Vector2(0, -1), mesh2D, horiPlayerBoundMat);

        ProjectileBoundaryEntity.Create(entityManager, new Vector2(-2.5f * boundaryOffset, 0), new Vector2(1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2.5f * boundaryOffset, 0), new Vector2(-1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, 1.5f * boundaryOffset), new Vector2(0, -1), mesh2D, horiProjectileBoundMat, 40.3f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, 1.5f * -boundaryOffset), new Vector2(0, 1), mesh2D, horiProjectileBoundMat, 40.3f, Color.clear);
        CardEntity.Create(entityManager, new Vector2(0,-15f), 0, 0, 0, cardLibrary[0].manaCost, mesh2D, cardLibrary[0].getMaterial());

        EventManager.instance.QueueEvent(new SoundEvent(playDeck1.GetPrimaryString(), "Intro"));
        EventManager.instance.QueueEvent(new SoundEvent(playDeck2.GetPrimaryString(), "Intro"));
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

    public int DrawCardFromDeck(int player, int cardSlot)
    {
       if(player == 1)
        {
            int nextCard = playDeck1.DrawCard();
            Debug.Log("NextCard: " + nextCard);
            if (nextCard > 0) CardEntity.Create(entityManager, new Vector2(-boundaryOffset - 7, -7.5f), nextCard, cardSlot, player, cardLibrary[nextCard].manaCost, mesh2D, cardLibrary[nextCard].getMaterial());
            //else playDeck1.Shuffle();
            return nextCard;
        }
       else
       {
            int nextCard = playDeck2.DrawCard();
            if (nextCard != 0) CardEntity.Create(entityManager, new Vector2(boundaryOffset - 7, -7.5f), nextCard, cardSlot, player, cardLibrary[nextCard].manaCost, mesh2D, cardLibrary[nextCard].getMaterial());
            //else playDeck2.Shuffle();
            return nextCard;
        }
    }
    public void Reshuffle(int player)
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
    public string[] DeckGenres()
    {
        return new string[] {playDeck1.GetPrimaryString(), playDeck2.GetPrimaryString()};
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

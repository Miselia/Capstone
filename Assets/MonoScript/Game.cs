using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Entities;
using Assets.Resources;
using Assets.MonoScript;

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
    [SerializeField] private List<Material> cardMaterialLibrary;

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

        PlayerEntity.Create(entityManager, new Vector2(-boundaryOffset,0), new Vector2(0, 0), playerRadius, 1, maxHealth, maxMana, manaRegen, mesh2D, playerMat);
        PlayerEntity.Create(entityManager, new Vector2(boundaryOffset, 0), new Vector2(0, 0), playerRadius, 2, maxHealth, maxMana, manaRegen, mesh2D, playerMat);
        
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

        playDeck1 = LobbyScript.p1Deck;
        playDeck2 = LobbyScript.p2Deck;

        ProjectileBoundaryEntity.Create(entityManager, new Vector2(-2 * boundaryOffset, 0), new Vector2(1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(2 * boundaryOffset, 0), new Vector2(-1, 0), mesh2D, vertProjectileBoundMat, 20.0f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, boundaryOffset), new Vector2(0, -1), mesh2D, horiProjectileBoundMat, 40.3f, Color.clear);
        ProjectileBoundaryEntity.Create(entityManager, new Vector2(0, -boundaryOffset), new Vector2(0, 1), mesh2D, horiProjectileBoundMat, 40.3f, Color.clear);
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
            if (nextCard != 0) CardEntity.Create(entityManager, new Vector2(-boundaryOffset - 7, -7.5f), nextCard, cardSlot, player, mesh2D, cardMaterialLibrary[nextCard]);
            //else playDeck1.Shuffle();
            return nextCard;
        }
       else
       {
            int nextCard = playDeck2.DrawCard();
            if (nextCard != 0) CardEntity.Create(entityManager, new Vector2(boundaryOffset - 7, -7.5f), nextCard, cardSlot, player, mesh2D, cardMaterialLibrary[nextCard]);
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

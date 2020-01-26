using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;

public class Spawner : MonoBehaviour, IGenericEventListener
{
    public int boundaryOffset = Constants.PlayerBoundaryOffset;
    [SerializeField] private Material normalMat;
    [SerializeField] private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.RegisterListener<SpawnEvent>(this);
        
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is SpawnEvent)
        {
            SpawnEvent se = (SpawnEvent) evt;
            int cardID = World.Active.EntityManager.GetComponentData<CardComp>(se.card).cardID;
            int playerID = World.Active.EntityManager.GetComponentData<CardComp>(se.card).player;
            int currentMana = World.Active.EntityManager.GetComponentData<PlayerComponent>(se.player).mana;
            Entity player = se.player;
            Entity card = se.card;
            spawn(cardID, playerID, currentMana, card, player);
            Debug.Log("Something is Spawned");
            return true;
        }
        return false;
    }

    public void spawn(int cardID, int playerID, int currentMana, Entity card, Entity player)
    {
        int manaCost;
        int positionX;
        if (playerID == 1) positionX = Constants.PlayerBoundaryOffset;
        else positionX = Constants.PlayerBoundaryOffset;
        switch (cardID)
        
        {
            case 1:
                manaCost = 2;
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("normal", new Vector2(positionX-5, -5), new Vector2(0, 3), 1.0f);
                    createBullet("normal", new Vector2(positionX, -5), new Vector2(0, 3), 0.5f);
                    createBullet("normal", new Vector2(positionX, -5), new Vector2(0, 3), 0.25f);
                    

                    adjustPlayerValues(player, -manaCost, 0);

                    World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));
                }
                break;
            case 2:
                manaCost = 2;
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("normal", new Vector2(positionX, 0), new Vector2(3, 0), 0.2f);
                    createBullet("normal", new Vector2(positionX, 0), new Vector2(-3, 0), 0.2f);
                    createBullet("normal", new Vector2(positionX, -5), new Vector2(0, 0.5f), 1.5f);
                   

                    adjustPlayerValues(player, -manaCost, 0);
                    World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));
                }
                break;
            case 3:
                manaCost = 2;
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("normal", new Vector2(positionX, 4), new Vector2(0, -1), 2.0f);

                    adjustPlayerValues(player, -manaCost, 0);
                    World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));
                }
                break;
            case 4:
                manaCost = 2;
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("normal", new Vector2(positionX, 5), new Vector2(-2, -2), 2.0f);
                    createBullet("normal", new Vector2(positionX, 5), new Vector2(2, -2), 2.0f);
                    createBullet("normal", new Vector2(positionX, -5), new Vector2(-2, 2), 2.0f);
                    createBullet("normal", new Vector2(positionX, -5), new Vector2(2, 2), 2.0f);

                    adjustPlayerValues(player, -manaCost, 0);
                    World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));
                }
                break;
        }
    }
    private void createBullet(string type, Vector2 position, Vector2 movementvector, float radius)
    {
        switch (type)
        {
            case "normal":
                ProjectileEntity.Create(World.Active.EntityManager, position, movementvector, radius, mesh, normalMat);
                break;
        }
    }
    private bool checkMana(int manaCost, int mana)
    {
        if ((mana - manaCost) < 0)
        {
            return false;
        }
        return true;
    }
    private void adjustPlayerValues(Entity player, int manaDelta, int healthDelta)
    {
        int currentHealth = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).healthRemaining;
        int currentMana = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).mana;
        int newHealth = currentHealth + healthDelta;
        int newMana = currentMana + manaDelta;
        int playerID = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).playerID;
        World.Active.EntityManager.SetComponentData<PlayerComponent>(player, new PlayerComponent(playerID, newHealth, newMana));

        EventManager.instance.QueueEvent(new UIUpdateEvent(newHealth, newMana, playerID));
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;

public class Spawner : MonoBehaviour, IGenericEventListener
{
    public int boundaryOffset = Constants.GameBoundaryOffset;
    [SerializeField] private List<Material> projectileMaterialLibrary;
    [SerializeField] private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.RegisterListener<SpawnEvent>(this);
        
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        Debug.Log("Handling Spawn Event");
        if (evt is SpawnEvent)
        {
            SpawnEvent se = (SpawnEvent) evt;
            Entity player = se.player;
            Entity card = se.card;
            
            spawn(card, player);
            Debug.Log("Something is Spawned");
            return true;
        }
        return false;
    }

    public void spawn( Entity card, Entity player)
    {
        int cardID = World.Active.EntityManager.GetComponentData<CardComp>(card).cardID;
        int playerID = World.Active.EntityManager.GetComponentData<CardComp>(card).player;
        float currentMana = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).mana;
        float manaCost  = World.Active.EntityManager.GetComponentData<CardComp>(card).manaCost;
        int positionX;
        if (playerID == 1) positionX = Constants.GameBoundaryOffset;
        else positionX = -Constants.GameBoundaryOffset;
        switch (cardID)
        
        {
            case 1:
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
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("fire", new Vector2(positionX, 0), new Vector2(3, 0), 0.2f);
                    createBullet("fire", new Vector2(positionX, 0), new Vector2(-3, 0), 0.2f);
                    createBullet("fire", new Vector2(positionX, -5), new Vector2(0, 0.5f), 1.5f);
                   

                    adjustPlayerValues(player, -manaCost, 0);
                    
                    World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));
                }
                break;
            case 3:
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("purple", new Vector2(positionX, 4), new Vector2(0, -1), 2.0f);

                    adjustPlayerValues(player, -manaCost, 0);
                    World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));
                }
                break;
            case 4:
                if (checkMana(manaCost, currentMana))
                {
                    createBullet("red", new Vector2(positionX, 5), new Vector2(-2, -2), 2.0f);
                    createBullet("red", new Vector2(positionX, 5), new Vector2(2, -2), 2.0f);
                    createBullet("red", new Vector2(positionX, -5), new Vector2(-2, 2), 2.0f);
                    createBullet("red", new Vector2(positionX, -5), new Vector2(2, 2), 2.0f);

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
                ProjectileEntity.Create(World.Active.EntityManager, position, movementvector, radius, mesh, projectileMaterialLibrary[0]);
                break;
            case "fire":
                ProjectileEntity.Create(World.Active.EntityManager, position, movementvector, radius, mesh, projectileMaterialLibrary[1]);
                break;
            case "purple":
                ProjectileEntity.Create(World.Active.EntityManager, position, movementvector, radius, mesh, projectileMaterialLibrary[2]);
                break;
            case "red":
                ProjectileEntity.Create(World.Active.EntityManager, position, movementvector, radius, mesh, projectileMaterialLibrary[3]);
                break;
        }
    }
    private bool checkMana(float manaCost, float mana)
    {
        if ((mana - manaCost) < 0)
        {
            return false;
        }
        return true;
    }
    private void adjustPlayerValues(Entity player, float manaDelta, int healthDelta)
    {

        //int[] values = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).adjustMana(manaDelta);
        /*
        int[] values = { 0, 0, 0 };
        values[1] = (int) Mathf.Floor(World.Active.EntityManager.GetComponentData<PlayerComponent>(player).mana);
        values[0] = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).healthRemaining;
        values[2] = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).playerID;

        EventManager.instance.QueueEvent(new UIUpdateEvent(values[0], values[1], values[2]));
        */
        World.Active.EntityManager.AddComponent(player, typeof(ManaDeltaComp));
        World.Active.EntityManager.SetComponentData(player, new ManaDeltaComp(manaDelta));
    }

}

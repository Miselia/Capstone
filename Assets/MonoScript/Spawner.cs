using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;

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
            
            spawn(card, player, 0, 60);
            Debug.Log("Something is Spawned");
            return true;
        }
        return false;
    }

    public void spawn( Entity card, Entity player, int fixedValue, int time)
    {
        int cardID = World.Active.EntityManager.GetComponentData<CardComp>(card).cardID;
        if (fixedValue != 0) cardID = fixedValue;
        int playerID = World.Active.EntityManager.GetComponentData<CardComp>(card).player;
        float currentMana = World.Active.EntityManager.GetComponentData<PlayerComponent>(player).mana;
        float manaCost  = World.Active.EntityManager.GetComponentData<CardComp>(card).manaCost;

        // If playerID = 1 then use Positive offset, else use negative offset
        // If in GameScene use GameBoundaryOffset, else use DeckBuilderBoundaryOffset
        int positionX = (playerID == 1) ? 
            (SceneManager.GetActiveScene().name.Equals("GameScene")) ? Constants.GameBoundaryOffset : Constants.DeckBuilderBoundaryOffset :
            (SceneManager.GetActiveScene().name.Equals("GameScene")) ? -Constants.GameBoundaryOffset : -Constants.DeckBuilderBoundaryOffset;
        if (checkMana(manaCost, currentMana) || fixedValue != 0)
        {
            int damage = -1;
            int timer = time;
            EventManager.instance.QueueEvent(new SoundEvent(0));
            if (World.Active.EntityManager.HasComponent<ValueIncreaseComp>(player))
            {
                World.Active.EntityManager.RemoveComponent<ValueIncreaseComp>(player);
                damage = damage * 2;
            }
            if (World.Active.EntityManager.HasComponent<DoubleCastComp>(player))
            {
                World.Active.EntityManager.RemoveComponent<DoubleCastComp>(player);
                spawn(card, player, cardID, timer+40);
            }

            switch (cardID)

            {
                case 1:
                    //Blood Boil
                        createBullet("normal", new Vector2(positionX - 5, -5), new Vector2(0, 3), 1.0f, damage, timer);
                        createBullet("normal", new Vector2(positionX, -5), new Vector2(0, 3), 0.5f, damage, timer);
                        createBullet("normal", new Vector2(positionX + 5, -5), new Vector2(0, 3), 0.25f, damage, timer);

                    break;

                case 2:
                    //Fire Bolt
                        createBullet("fire", new Vector2(positionX, 0), new Vector2(3, 0), 0.2f, damage, timer);
                        createBullet("fire", new Vector2(positionX, 0), new Vector2(-3, 0), 0.2f, damage, timer);
                        createBullet("fire", new Vector2(positionX, -5), new Vector2(0, 0.5f), 1.5f, damage, timer);

                    break;

                case 3:
                    //Plasma Bolt
                        createBullet("purple", new Vector2(positionX, 4), new Vector2(0, -1), 2.0f, damage, timer);

                    break;

                case 4:
                    //Red Coins
                        createBullet("red", new Vector2(positionX, 5), new Vector2(-2, -2), 1.0f, damage, timer);
                        createBullet("red", new Vector2(positionX, 5), new Vector2(2, -2), 1.0f, damage, timer);
                        createBullet("red", new Vector2(positionX, -5), new Vector2(-2, 2), 1.0f, damage, timer);
                        createBullet("red", new Vector2(positionX, -5), new Vector2(2, 2), 1.0f, damage, timer);

                    break;

                case 5:
                        //Glipse into the Ether
                        damage = 0;
                        if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -Constants.DeckBuilderBoundaryOffset;

                        createBullet("etherBuff", new Vector2(-positionX + 5, +5), new Vector2(0, 0), 0.5f, damage, timer);
                        createBullet("etherBuff", new Vector2(-positionX - 5, -5), new Vector2(0, 0), 0.5f, damage, timer);

                    break;

                case 6:
                        //Wandering Woodsprite
                        damage = -damage;
                        if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -Constants.DeckBuilderBoundaryOffset;

                        createBullet("woodsprite", new Vector2(-positionX * 1.8f, 0), new Vector2(positionX / 2, 0), 0.5f, damage, timer);

                    break;
                case 7:
                    //Akashic Records
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -Constants.DeckBuilderBoundaryOffset;

                    int num1 = 7;
                    int num2 = 7;
                    while (num1 == 7)
                    {
                        num1 = Random.Range(1, 7);
                    }
                    while (num2 == 7)
                    {
                        num2 = Random.Range(1, 7);
                    }

                    spawn(card, player, num1, timer);
                    spawn(card, player, num2, timer+20);

                    break;
                case 8:
                    //Well of Knowledge
                    createBullet("water", new Vector2(positionX, -5), new Vector2(1, 1), 0.5f, damage, timer);
                    createBullet("water", new Vector2(positionX, -5), new Vector2(-1, 1), 0.5f, damage, timer);
                    createBullet("water", new Vector2(positionX, -5), new Vector2(0, 1), 0.5f, damage, timer);

                    World.Active.EntityManager.AddComponent(player, typeof(ValueIncreaseComp));
                    World.Active.EntityManager.SetComponentData(player, new ValueIncreaseComp(true));

                    break;
                case 9:
                    //Magic Records

                    World.Active.EntityManager.AddComponent(player, typeof(DoubleCastComp));
                    World.Active.EntityManager.SetComponentData(player, new DoubleCastComp(true));

                    break;
                case 10:
                    //Nano Barrier
                    createBullet("barrier", new Vector2(positionX - 0.1f, 0), new Vector2(-1, 0), 1f, damage, timer);
                    createBullet("barrier", new Vector2(positionX + 0.1f, 0), new Vector2(1, 0), 1f, damage, timer);

                    break;
                case 11:
                    //Gravity Well
                    createBullet("gravityWell",new Vector2(positionX, 0), new Vector2(0, 0), 1f, damage, 0);

                    break;
                case 12:
                    //Electromagnetic Projectiles
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(0.5f, 1), 0.5f, damage, timer);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(-0.5f, 1), 0.5f, damage, timer);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(1, 1), 0.5f, damage, timer);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(-1, 1), 0.5f, damage, timer);

                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(0.5f, 1), 0.5f, damage, timer+ 80);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(-0.5f, 1), 0.5f, damage, timer + 80);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(1, 1), 0.5f, damage, timer + 80);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(-1, 1), 0.5f, damage, timer + 80);

                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(0.5f, 1), 0.5f, damage, timer + 160);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(-0.5f, 1), 0.5f, damage, timer + 160);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(1, 1), 0.5f, damage, timer + 160);
                    createBullet("eProjectile", new Vector2(positionX, -5), new Vector2(-1, 1), 0.5f, damage, timer + 160);
                    break;
                case 13:

                    //Overcharge
                    foreach (Entity e in World.Active.EntityManager.GetAllEntities())
                    {
                        if(World.Active.EntityManager.HasComponent<ProjectileComponent>(e) && World.Active.EntityManager.HasComponent<MovementComponent>(e))
                        {
                            World.Active.EntityManager.AddComponent(e, typeof(ProjectileSpeedBuffComp));
                            World.Active.EntityManager.SetComponentData(e, new ProjectileSpeedBuffComp(3,180,new Vector2(0,0)));
                        }
                    }
                    break;
                case 14:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    //Overcharge
                    foreach (Entity e in World.Active.EntityManager.GetAllEntities())
                    {
                        if (World.Active.EntityManager.HasComponent<ProjectileComponent>(e) && World.Active.EntityManager.HasComponent<MovementComponent>(e))
                        {
                            World.Active.EntityManager.AddComponent(e, typeof(ProjectileSpeedBuffComp));
                            World.Active.EntityManager.SetComponentData(e, new ProjectileSpeedBuffComp(0, 240, new Vector2(0, 0)));
                        }
                    }
                    break;
            }
            if (fixedValue == 0)
            {
                adjustPlayerValues(player, -manaCost, 0);
                World.Active.EntityManager.AddComponent(card, typeof(DeleteComp));

            }
            
        }
    }
    

    /*public bool playCard(int cardID, int playerID, float currentMana, float manaCost, float positionX)
    {

    }*/

    private void createBullet(string type, Vector2 position, Vector2 movementvector, float radius, int damage, int timer)
    {
        switch (type)
        {
            case "normal":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[0]);
                break;
            case "fire":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[1]);
                break;
            case "purple":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[2]);
                break;
            case "red":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[3]);
                break;
            case "woodsprite":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[5]);
                break;
            case "etherBuff":
                Entity ether = ProjectileEntity.Create(World.Active.EntityManager, 0, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[4]);
                World.Active.EntityManager.AddComponent(ether, typeof(ManaRegenBuffComp));
                World.Active.EntityManager.SetComponentData(ether, new ManaRegenBuffComp(0.5f,120));
                World.Active.EntityManager.AddComponent(ether, typeof(DeleteComp));
                World.Active.EntityManager.SetComponentData(ether, new DeleteComp(300));

                break;
            case "water":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[6]);
                break;
            case "barrier":
                Entity barrier = PlayerBoundaryEntity.Create(World.Active.EntityManager, position, movementvector, mesh, projectileMaterialLibrary[7]);
                World.Active.EntityManager.AddComponent(barrier, typeof(DeleteComp));
                World.Active.EntityManager.SetComponentData(barrier, new DeleteComp(420));
                break;
            case "gravityWell":
                Entity gravity = ProjectileEntity.Create(World.Active.EntityManager, 0, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[8]);
                World.Active.EntityManager.RemoveComponent(gravity, typeof(ProjectileComponent));
                World.Active.EntityManager.AddComponent(gravity, typeof(GravityComponent));
                World.Active.EntityManager.SetComponentData(gravity, new GravityComponent(7,0.2f));
                World.Active.EntityManager.AddComponent(gravity, typeof(DeleteComp));
                World.Active.EntityManager.SetComponentData(gravity, new DeleteComp(300));
                break;
            case "eProjectile":
                ProjectileEntity.Create(World.Active.EntityManager, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[9]);
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

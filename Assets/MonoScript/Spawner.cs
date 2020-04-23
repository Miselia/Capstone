﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;
using Unity.Transforms;
using Assets.MonoScript;
using System.Linq;

public class Spawner : MonoBehaviour, IGenericEventListener
{
    public int boundaryOffset = Constants.GameBoundaryOffset;
    private EntityManager em;
    [SerializeField] private List<Material> projectileMaterialLibrary;
    [SerializeField] private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.RegisterListener<SpawnEvent>(this);
        EventManager.instance.RegisterListener<CreateProjectileEvent>(this);
        em = World.Active.EntityManager;
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        //Debug.Log("Handling Spawn Event");
        if (evt is SpawnEvent)
        {
            SpawnEvent se = (SpawnEvent) evt;
            
            spawn(se.card, se.player, 0, 100, se.opponent);
            //Debug.Log("Something is Spawned");
            return true;
        }
        if (evt is CreateProjectileEvent)
        {
            CreateProjectileEvent cpe = (CreateProjectileEvent) evt;

            Debug.Log("Create projectile listened to");
            createBullet(cpe.type, cpe.position, cpe.movementVector, cpe.radius, -cpe.damage, cpe.timer);
            return true;
        }
        return false;
    }

    public void spawn( Entity card, Entity player, int fixedValue, int time, Entity opponent)
    {
        int cardID = em.GetComponentData<CardComp>(card).cardID;
        if (fixedValue != 0) cardID = fixedValue;
        int playerID = em.GetComponentData<CardComp>(card).player;
        float currentMana = em.GetComponentData<PlayerComponent>(player).mana;
        float manaCost  = em.GetComponentData<CardComp>(card).manaCost;

        // If playerID = 1 then use Positive offset, else use negative offset
        // If in GameScene use GameBoundaryOffset, else use DeckBuilderBoundaryOffset
        int positionX = (playerID == 1) ? 
            (SceneManager.GetActiveScene().name.Equals("GameScene")) ? Constants.GameBoundaryOffset : Constants.DeckBuilderBoundaryOffset :
            (SceneManager.GetActiveScene().name.Equals("GameScene")) ? -Constants.GameBoundaryOffset : -Constants.DeckBuilderBoundaryOffset;
        if (checkMana(manaCost, currentMana) || fixedValue != 0)
        {
            int damage = -1;
            int timer = time;
            //EventManager.instance.QueueEvent(new SoundEvent(0));
            /*
             * Changed logic to only apply double damage when a spell is cast that actually deals damage using "CheckValueIncreaseComp" in respective 'case' blocks
             * if (em.HasComponent<ValueIncreaseComp>(player))
            {
                em.RemoveComponent<ValueIncreaseComp>(player);
                damage = damage * 2;
            }*/
            if (em.HasComponent<DoubleCastComp>(player))
            {
                em.RemoveComponent<DoubleCastComp>(player);
                spawn(card, player, cardID, timer+100, opponent);
            }

            switch (cardID)

            {
                case 1:
                    //Blood Boil
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2; 
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy","BuffSelf"));
                    createBullet("normal", new Vector2(positionX - 5, -5), new Vector2(0, 3), 1.0f, damage, timer);
                    createBullet("normal", new Vector2(positionX, -5), new Vector2(0, 3), 0.5f, damage, timer);
                    createBullet("normal", new Vector2(positionX + 5, -5), new Vector2(0, 3), 0.25f, damage, timer);
                    break;

                case 2:
                    //Fire Bolt
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy","SmallCard"));
                    createBullet("fire", new Vector2(positionX+6, 0), new Vector2(-3, 0), 0.3f, damage, timer);
                    createBullet("fire", new Vector2(positionX-6, 0), new Vector2(3, 0), 0.3f, damage, timer);
                    createBullet("fire", new Vector2(positionX, -6), new Vector2(0, 0.5f), 1.5f, damage, timer);
                    break;

                case 3:
                    //Plasma Bolt
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    createBullet("purple", new Vector2(positionX, 4), new Vector2(0, -1), 2.0f, damage, timer);
                    break;

                case 4:
                    //Red Coins
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    createBullet("red", new Vector2(positionX, 5), new Vector2(0, -2), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX+5, 0), new Vector2(-2, 0), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX, -5), new Vector2(0, 2), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX-5, 0), new Vector2(2, 0), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX-5, 5), new Vector2(2, -2), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX+5, 5), new Vector2(-2, -2), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX-5, -5), new Vector2(2, 2), 0.75f, damage, timer);
                    createBullet("red", new Vector2(positionX+5, -5), new Vector2(-2, 2), 0.75f, damage, timer);
                    break;

                case 5:
                    //Glipse into the Ether
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "BuffSelf"));
                    damage = 0;
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -Constants.DeckBuilderBoundaryOffset;

                    createBullet("etherBuff", new Vector2(-positionX + 5, +5), new Vector2(0, 0), 0.5f, damage, timer);
                    createBullet("etherBuff", new Vector2(-positionX - 5, -5), new Vector2(0, 0), 0.5f, damage, timer);
                    break;

                case 6:
                    //Wandering Woodsprite
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    damage = -damage;
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -Constants.DeckBuilderBoundaryOffset;

                    createBullet("woodsprite", new Vector2(-positionX * 1.8f, 0), new Vector2(positionX / 2, 0), 0.5f, damage, timer);
                    break;
                case 7:
                    //Akashic Records
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "LargeCard"));
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -positionX;

                    int num1 = 7;
                    int num2 = 7;
                    while (num1 == 7)
                    {
                        num1 = Random.Range(1, 18);
                    }
                    while (num2 == 7)
                    {
                        num2 = Random.Range(1, 18);
                    }

                    spawn(card, player, num1, timer, opponent);
                    spawn(card, player, num2, timer+20, opponent);

                    break;
                case 8:
                    //Well of Knowledge
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    createBullet("water", new Vector2(positionX, -5), new Vector2(1, 1), 0.5f, damage, timer);
                    createBullet("water", new Vector2(positionX, -5), new Vector2(-1, 1), 0.5f, damage, timer);
                    createBullet("water", new Vector2(positionX, -5), new Vector2(0, 1), 0.5f, damage, timer);

                    if (!em.HasComponent<ValueIncreaseComp>(player))
                    {
                        em.AddComponent(player, typeof(ValueIncreaseComp));
                        em.SetComponentData<ValueIncreaseComp>(player, new ValueIncreaseComp(1));
                    }

                    ValueIncreaseComp val = em.GetComponentData<ValueIncreaseComp>(player);
                    val.multiplier *= 2;
                    em.SetComponentData<ValueIncreaseComp>(player, val);

                    break;
                case 9:
                    //Magic Records
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "BuffProjectile"));
                    em.AddComponent(player, typeof(DoubleCastComp));
                    em.SetComponentData(player, new DoubleCastComp(true));

                    break;
                case 10:
                    //Nano Barrier
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    createBullet("barrier", new Vector2(positionX - 0.1f, 0), new Vector2(-1, 0), 1f, damage, timer, em.GetComponentData<PlayerComponent>(opponent).playerID);
                    createBullet("barrier", new Vector2(positionX + 0.1f, 0), new Vector2(1, 0), 1f, damage, timer, em.GetComponentData<PlayerComponent>(opponent).playerID);
                    break;
                case 11:
                    //Gravity Well
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "LargeCard"));
                    createBullet("gravityWell",new Vector2(positionX, 0), new Vector2(0, 0), 1f, damage, 0);

                    break;
                case 12:
                    //Electromagnetic Projectiles
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "LargeCard"));
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
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "BuffProjectile"));
                    foreach (Entity e in em.GetAllEntities())
                    {
                        if(em.HasComponent<ProjectileComponent>(e) && em.HasComponent<MovementComponent>(e))
                        {
                            em.AddComponent(e, typeof(ProjectileSpeedBuffComp));
                            em.SetComponentData(e, new ProjectileSpeedBuffComp(3,180,new Vector2(0,0)));
                        }
                    }
                    break;
                case 14:
                    // Lead Rain
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    Vector2 down = new Vector2(0, -2f);
                    createBullet("bullet", new Vector2(positionX - 3, 5), down, 0.5f, damage, timer);
                    createBullet("bullet", new Vector2(positionX, 5), down, 0.5f, damage, timer);
                    createBullet("bullet", new Vector2(positionX + 3, 5), down, 0.5f, damage, timer);

                    createBullet("bullet", new Vector2(positionX - 4.5f, 5), down, 0.5f, damage, timer + 100);
                    createBullet("bullet", new Vector2(positionX - 1.5f, 5), down, 0.5f, damage, timer + 100);
                    createBullet("bullet", new Vector2(positionX + 1.5f, 5), down, 0.5f, damage, timer + 100);
                                                                                          
                    createBullet("bullet", new Vector2(positionX - 3, 5), down, 0.5f, damage, timer + 200);
                    createBullet("bullet", new Vector2(positionX, 5), down, 0.5f, damage, timer + 200);
                    createBullet("bullet", new Vector2(positionX + 3, 5), down, 0.5f, damage, timer + 200);
                                                                                          
                    createBullet("bullet", new Vector2(positionX - 1.5f, 5), down, 0.5f, damage, timer + 300);
                    createBullet("bullet", new Vector2(positionX + 1.5f, 5), down, 0.5f, damage, timer + 300);
                    createBullet("bullet", new Vector2(positionX + 4.5f, 5), down, 0.5f, damage, timer + 300);
                    break;
                case 15:
                    // Spray and Pray
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "SmallCard"));
                    int speed = 3;
                    int direction = -positionX / Mathf.Abs(positionX);
                    Vector2 bottomCorner = new Vector2(positionX - direction * 5, -5);
                    createBullet("bullet", bottomCorner, new Vector2(direction * 0.1f * speed, 0.9f * speed), 0.5f, damage, timer);
                    //createBullet("bullet", bottomCorner, new Vector2(direction * 0.2f * speed, 0.8f * speed), 0.5f, damage, timer + 20);
                    createBullet("bullet", bottomCorner, new Vector2(direction * 0.3f * speed, 0.7f * speed), 0.5f, damage, timer + 40);
                    //createBullet("bullet", bottomCorner, new Vector2(direction * 0.4f * speed, 0.6f * speed), 0.5f, damage, timer + 60);
                    createBullet("bullet", bottomCorner, new Vector2(direction * 0.5f * speed, 0.5f * speed), 0.5f, damage, timer + 80);
                    //createBullet("bullet", bottomCorner, new Vector2(direction * 0.6f * speed, 0.4f * speed), 0.5f, damage, timer + 100);
                    createBullet("bullet", bottomCorner, new Vector2(direction * 0.7f * speed, 0.3f * speed), 0.5f, damage, timer + 120);
                    //createBullet("bullet", bottomCorner, new Vector2(direction * 0.8f * speed, 0.2f * speed), 0.5f, damage, timer + 140);
                    createBullet("bullet", bottomCorner, new Vector2(direction * 0.9f * speed, 0.1f * speed), 0.5f, damage, timer + 180);
                    break;
                case 16:
                    // Well Oiled Machine
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "BuffSelf"));
                    damage = 0;
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder")) positionX = -Constants.DeckBuilderBoundaryOffset;

                    createBullet("oil", new Vector2(-positionX, 0), new Vector2(), 0.5f, damage, timer);
                    break;
                case 17:
                    // Gear Box
                    if (CheckValueIncreaseComp(player))
                    {
                        em.RemoveComponent<ValueIncreaseComp>(player);
                        damage *= 2;
                    }
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "LargeCard"));
                    createBullet("gear", new Vector2(positionX - 5, 5), new Vector2(0, 5), 0.75f, damage, timer);
                    break;
                case 18:
                    //Chronosis
                    EventManager.instance.QueueEvent(new SoundEvent("Fantasy", "BuffProjectile"));
                    foreach (Entity e in em.GetAllEntities())
                    {
                        if (em.HasComponent<ProjectileComponent>(e) && em.HasComponent<MovementComponent>(e))
                        {
                            em.AddComponent(e, typeof(IsBuffedComponent));
                            em.AddComponent(e, typeof(ProjectileSpeedBuffComp));
                            em.SetComponentData(e, new ProjectileSpeedBuffComp(0, 240, new Vector2(0, 0)));
                        }
                    }
                    break;
                case 19:
                    // Wall Spikes
                    // Spawn one wall spike onto each player boundary on opponent's side of field
                    int oppSide;
                    if(SceneManager.GetActiveScene().name.Equals("DeckBuilder"))
                    {
                        oppSide = 1;
                    }
                    else
                    {
                        oppSide = em.GetComponentData<PlayerComponent>(opponent).playerID;
                    }
                    foreach (Entity e in em.GetAllEntities())
                    {
                        if(CheckValueIncreaseComp(player))
                        {
                            em.RemoveComponent<ValueIncreaseComp>(player);
                            damage *= 2;
                        }
                        // short circuit
                        if (em.HasComponent<PlayerBoundaryComponent>(e))
                        {
                            PlayerBoundaryComponent pbc = em.GetComponentData<PlayerBoundaryComponent>(e);
                            if(pbc.side == oppSide)
                            {
                                Vector2 location = new Vector2(em.GetComponentData<Translation>(e).Value.x, em.GetComponentData<Translation>(e).Value.y);
                                location += new Vector2(pbc.Normal.x, pbc.Normal.y);
                                
                                Vector2 mod = new Vector2(pbc.Normal.y, pbc.Normal.x);
                                mod *= 3;
                                location += mod;
                                createBullet("spike", location, new Vector2(), 1, damage, timer, -1, new Vector2(pbc.Normal.x, -pbc.Normal.y));
                            }
                        }
                    }
                    break;
                case 20:
                    // Viper's Curse
                    // Attach a new "ViperDebuffComponent" to the opponent (self if DeckBuilder) and have it managed by the "BuffSystem"
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder"))
                    {
                        em.AddComponent<IsBuffedComponent>(player);
                        em.AddComponent<ViperCurseComponent>(player);
                        em.SetComponentData<ViperCurseComponent>(player, new ViperCurseComponent(600));
                    }
                    else
                    {
                        em.AddComponent<IsBuffedComponent>(opponent);
                        em.AddComponent<ViperCurseComponent>(opponent);
                        em.SetComponentData<ViperCurseComponent>(opponent, new ViperCurseComponent(600));
                    }
                    break;
                case 21:
                    // Create a projectile without a collision component (like Gravity), adding only a delete component of a rather short time
                    // Also add sound effect on spawn
                    damage = 0;
                    createBullet("jumpScare", new Vector2(positionX, 0), new Vector2(), 24, damage, timer);
                    EventManager.instance.QueueEvent(new SoundEvent("Other", "Doot"));
                    break;
                case 22:
                    // Start spawn animation of flame pillar at bottom of opponent's field based on user position
                    Vector2 pillarPos;
                    if(SceneManager.GetActiveScene().name.Equals("DeckBuilder"))
                    {
                        pillarPos = new Vector2(em.GetComponentData<Translation>(player).Value.x, 0);
                    }
                    else
                    {
                        float offset = (playerID == 1) ?
                            em.GetComponentData<Translation>(player).Value.x + 7 :
                            em.GetComponentData<Translation>(player).Value.x - 7 ;

                        pillarPos = (playerID == 1) ?
                            new Vector2(Constants.GameBoundaryOffset + offset, 0) :
                            new Vector2(Constants.GameBoundaryOffset - offset, 0) ;
                    }
                    createBullet("lightCigar", pillarPos, new Vector2(), 1.4f, damage, timer);
                    createBullet("", new Vector2(pillarPos.x, 1.5f), new Vector2(), 1.4f, damage, timer);
                    createBullet("", new Vector2(pillarPos.x, 3), new Vector2(), 1.4f, damage, timer);
                    createBullet("", new Vector2(pillarPos.x, 4.5f), new Vector2(), 1.4f, damage, timer);
                    createBullet("", new Vector2(pillarPos.x, - 1.5f), new Vector2(), 1.4f, damage, timer);
                    createBullet("", new Vector2(pillarPos.x, - 3), new Vector2(), 1.4f, damage, timer);
                    createBullet("", new Vector2(pillarPos.x, - 4.5f), new Vector2(), 1.4f, damage, timer);
                    //gameObject.GetComponent("IGame").AddCard
                    var s1 = FindObjectsOfType<MonoBehaviour>().OfType<IGame>();
                    foreach(IGame game in s1)
                    {
                        //Debug.Log("Next cigar card added to hand Player: " + playerID + ", Card Slot: " + em.GetComponentData<CardComp>(card).cardSlot);
                        game.AddCardToHandFromCardLibrary(playerID, em.GetComponentData<CardComp>(card).cardSlot, 23);
                    }
                    break;
                case 23:
                    // Spawn projectiles from top of screen (based on user position in the future?)
                    createBullet("flickCigar", new Vector2(positionX, 6), new Vector2(-.75f, -1), 0.5f, damage, timer);
                    createBullet("flickCigar", new Vector2(positionX, 6), new Vector2(-.5f, -1), 0.5f, damage, timer);
                    createBullet("flickCigar", new Vector2(positionX, 6), new Vector2(.75f, -1), 0.5f, damage, timer);
                    createBullet("flickCigar", new Vector2(positionX, 6), new Vector2(.5f, -1), 0.5f, damage, timer);
                    createBullet("flickCigar", new Vector2(positionX, 6), new Vector2(0, -1), 0.5f, damage, timer);
                    var s2 = FindObjectsOfType<MonoBehaviour>().OfType<IGame>();
                    foreach(IGame game in s2)
                    {
                        //Debug.Log("Next cigar card added to hand Player: " + playerID + ", Card Slot: " + em.GetComponentData<CardComp>(card).cardSlot);
                        game.AddCardToHandFromCardLibrary(playerID, em.GetComponentData<CardComp>(card).cardSlot, 24);
                    }
                    break;
                case 24:
                    // After delay, smash cigar, starting at top of opponent's side, based on user poition
                    Vector2 cigarPos;
                    if (SceneManager.GetActiveScene().name.Equals("DeckBuilder"))
                    {
                        cigarPos = new Vector2(em.GetComponentData<Translation>(player).Value.x, 0);
                    }
                    else
                    {
                        float offset = (playerID == 1) ?
                            em.GetComponentData<Translation>(player).Value.x + 7 :
                            em.GetComponentData<Translation>(player).Value.x - 7;

                        cigarPos = (playerID == 1) ?
                            new Vector2(Constants.GameBoundaryOffset + offset, 6) :
                            new Vector2(Constants.GameBoundaryOffset - offset, 6);
                    }
                    createBullet("smashCigar", cigarPos, new Vector2(0, -1), 1, damage, timer);
                    break;
                case 25:
                    // First cast begins targeting system, second cast fires the missile in the direction from 1st to 2nd cast
                    // Explodes on wall creating fragmented projectiles
                    break;
            }
            if (fixedValue == 0)
            {
                adjustPlayerValues(player, -manaCost, 0);
                em.AddComponent(card, typeof(DeleteComp));
            }
        }
    }

    private bool CheckValueIncreaseComp(Entity e)
    {
        if (em.HasComponent<ValueIncreaseComp>(e))
            return true;
        else
            return false;
    }

    private void createBullet(string type, Vector2 position, Vector2 movementvector, float radius, int damage, int timer, int side = -1, Vector2 initialRotation = new Vector2())
    {
        switch (type)
        {
            case "normal":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[0]);
                break;
            case "fire":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[1]);
                break;
            case "purple":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[2]);
                break;
            case "red":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[3]);
                break;
            case "woodsprite":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[5]);
                break;
            case "etherBuff":
                Entity ether = ProjectileEntity.Create(em, 0, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[4]);
                em.AddComponent(ether, typeof(ManaRegenBuffComp));
                em.SetComponentData(ether, new ManaRegenBuffComp(0.5f,120,120));
                em.AddComponent(ether, typeof(DeleteComp));
                em.SetComponentData(ether, new DeleteComp(300));
                break;
            case "water":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[6]);
                break;
            case "barrier":
                Entity barrier = PlayerBoundaryEntity.Create(em, position, movementvector, mesh, projectileMaterialLibrary[7], side);
                em.AddComponent(barrier, typeof(DeleteComp));
                em.SetComponentData(barrier, new DeleteComp(420));
                break;
            case "gravityWell":
                Entity gravity = ProjectileEntity.Create(em, 0, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[8], 0x00);
                em.RemoveComponent(gravity, typeof(ProjectileComponent));
                //em.RemoveComponent(gravity, typeof(CollisionComponent));
                em.AddComponent(gravity, typeof(GravityComponent));
                em.SetComponentData(gravity, new GravityComponent(7,0.2f));
                em.AddComponent(gravity, typeof(DeleteComp));
                em.SetComponentData(gravity, new DeleteComp(300));
                break;
            case "eProjectile":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[9]);
                break;
            case "bullet":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[10]);
                break;
            case "oil":
                Entity oil = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[11]);
                em.AddComponent(oil, typeof(DeleteComp));
                em.SetComponentData(oil, new DeleteComp(300));
                break;
            case "gear":
                Entity gear = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[12], 0x09, false);
                em.AddComponent(gear, typeof(ProjectileCollisionWithPlayerBoundaryComponent));
                em.SetComponentData(gear, new ProjectileCollisionWithPlayerBoundaryComponent(Constants.GearID));
                //em.AddComponent(gear, typeof(DeleteComp));
                //em.SetComponentData(gear, new DeleteComp(1000));
                break;
            case "spike":
                Entity spike = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[13], 0x03, false, initialRotation);
                em.AddComponent(spike, typeof(DeleteComp));
                em.SetComponentData(spike, new DeleteComp(360));
                break;
            case "Viper":
                Entity poison = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[14]);
                em.AddComponent<DeleteComp>(poison);
                em.SetComponentData(poison, new DeleteComp(300));
                break;
            case "jumpScare":
                Entity scare = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[15], 0x00);
                em.RemoveComponent(scare, typeof(SpawnDelayComp));
                //em.RemoveComponent(scare, typeof(ProjectileComponent));
                //em.RemoveComponent(scare, typeof(CollisionComponent));
                em.AddComponent(scare, typeof(DeleteComp));
                em.SetComponentData(scare, new DeleteComp(100));
                break;
            case "lightCigar":
                Entity flame = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[16], 0x03, true, new Vector2(), 4);
                em.AddComponent<DeleteComp>(flame);
                em.SetComponentData<DeleteComp>(flame, new DeleteComp(timer + 5));
                // consider increasing time, would require colliding with linked projectiles to also delete the others, ehhhh
                break;
            case "flickCigar":
                ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[17]);
                break;
            case "smashCigar":
                Entity cigar = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, projectileMaterialLibrary[18], 0x08, true, new Vector2(), 6);
                em.AddComponent(cigar, typeof(ProjectileCollisionWithPlayerBoundaryComponent));
                em.SetComponentData(cigar, new ProjectileCollisionWithPlayerBoundaryComponent(Constants.CigarID));
                break;
            default:
                // Draws invisible projectile that gets deleted immediately
                Material mat = projectileMaterialLibrary[21];
                mat.color = Color.clear;
                Entity invis = ProjectileEntity.Create(em, damage, position, movementvector, radius, timer, mesh, mat);
                em.AddComponent<DeleteComp>(invis);
                em.SetComponentData<DeleteComp>(invis, new DeleteComp(timer + 5));
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
        //int[] values = em.GetComponentData<PlayerComponent>(player).adjustMana(manaDelta);
        /*
        int[] values = { 0, 0, 0 };
        values[1] = (int) Mathf.Floor(em.GetComponentData<PlayerComponent>(player).mana);
        values[0] = em.GetComponentData<PlayerComponent>(player).healthRemaining;
        values[2] = em.GetComponentData<PlayerComponent>(player).playerID;

        EventManager.instance.QueueEvent(new UIUpdateEvent(values[0], values[1], values[2]));
        */
        em.AddComponent(player, typeof(ManaDeltaComp));
        em.SetComponentData(player, new ManaDeltaComp(manaDelta));
    }

}

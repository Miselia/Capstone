using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Spawner : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private Material normalMat;
    [SerializeField] private Mesh mesh;
    private EntityManager em;
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.RegisterListener<SpawnEvent>(this);
        game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        //em = 
        
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        Debug.Log("Handling Spawn Event");
        if (evt is SpawnEvent)
        {
            SpawnEvent se = (SpawnEvent) evt;
            spawn(se.cardID, se.player);
            return true;
        }
        return false;
    }

    public void spawn(int cardID, int playerID)
    {
        switch (cardID)
        {
            case 1:
                createBullet("normal", new Vector2(-5, -5), new Vector2(0, 3), 1.0f);
                createBullet("normal", new Vector2(0, -5), new Vector2(0, 3), 0.5f);
                createBullet("normal", new Vector2(5, -5), new Vector2(0, 3), 0.25f);
                break;
            case 2:
                createBullet("normal", new Vector2(-5, 0), new Vector2(3, 0), 0.2f);
                createBullet("normal", new Vector2(5, 0), new Vector2(-3, 0), 0.2f);
                createBullet("normal", new Vector2(0, -5), new Vector2(0, 0.5f), 1.5f);
                break;
            case 3:
                createBullet("normal", new Vector2(0, 4), new Vector2(0, -1), 2.0f);
                break;
            case 4:
                createBullet("normal", new Vector2(5, 5), new Vector2(-2, -2), 2.0f);
                createBullet("normal", new Vector2(-5, 5), new Vector2(2, -2), 2.0f);
                createBullet("normal", new Vector2(5, -5), new Vector2(-2, 2), 2.0f);
                createBullet("normal", new Vector2(-5, -5), new Vector2(2, 2), 2.0f);
                break;
        }
    }
    private void createBullet(string type, Vector2 position, Vector2 movementvector, float radius)
    {
        switch (type)
        {
            case "normal":
                ProjectileEntity.Create(game.getEntityManager(), position, movementvector, radius, mesh, normalMat);
                break;
        }
    }
}

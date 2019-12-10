using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Spawner
{
    
    private EntityManager em;
    private Material normalMat;
    private Mesh mesh;
    // Start is called before the first frame update
    public Spawner(EntityManager em, Mesh mesh, Material normalMaterial)
    {
        this.em = em;
        this.normalMat = normalMaterial;
        this.mesh = mesh;
    }
    
    public void spawn(int cardID, int playerID)
    {
        switch (cardID)
        {
            case 1:
                createBullet("normal", new Vector2(-5, -5), new Vector2(0, 1), 1.0f);
                createBullet("normal", new Vector2(0, -5), new Vector2(0, 1), 0.5f);
                createBullet("normal", new Vector2(5, -5), new Vector2(0, 1), 0.25f);
                break;
        }
    }
    private void createBullet(string type, Vector2 position, Vector2 movementvector, float radius)
    {
        switch (type)
        {
            case "normal":
                ProjectileEntity.Create(em, position, movementvector, radius, mesh, normalMat);
                break;
        }
    }
}

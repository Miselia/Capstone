using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Game : MonoBehaviour
{
    private EntityManager entityManager;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    [SerializeField] private Mesh projectileMesh;
    [SerializeField] private Material projectileMat;
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.Active.EntityManager;
<<<<<<< HEAD
        PlayerEntity.Create(entityManager, new Vector2(0,2), new Vector2(0, 1), 1, 3, mesh,mat);
        ProjectileEntity.Create(entityManager, new Vector2(5,0), new Vector2(1, 0), 10.0f, projectileMesh, projectileMat);
=======
        PlayerEntity.Create(entityManager, new Vector2(0,0), new Vector2(0, 0), 10.0f, 1, 3, mesh,mat);
        ProjectileEntity.Create(entityManager, new Vector2(1000,1000), new Vector2(5, 0), 10.0f, projectileMesh, projectileMat);
>>>>>>> bf301571769521d317667a3ffc3825c0b294e06e
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public EntityManager getEntityManager()
    {
        return entityManager;
    }
}

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
        PlayerEntity.Create(entityManager, new Vector2(0, 0), mesh,mat);
        ProjectileEntity.Create(entityManager, new Vector2(5, 0), projectileMesh, projectileMat);
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

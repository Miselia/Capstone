using Unity.Entities;

public struct CollisionComponent : IComponentData
{
    public float collisionRadius;
    public float width;
    public byte mask;

    /* Collisison detection is based off of the byte mask assigned to the CollisionComponent. Masks are compared using &, meaning collision is
     * only detected if the masks return anything but 0.
     * Examples
     * xxxx x1x1 (Player Entitiy)
     * xxxx xx11 (Projectile Entity)
     *      These will collide
     * xxxx x1x1 (Player Entity)
     * xxxx xx1x (Projectile Boundary)
     *      These will not collide
     *      
     * All Masks
     * Each collision mask has 8 bits (1 byte)
     * What other entities this entity collides with is based on the bits given. Currently entities of the same type cannot collide.
     * xxxx xxx1 Is for Projectiles and Player collision.
     * xxxx xx1x Is for Colliding with Projectile Boundaries. This is the mask of Projectile Boundaries.
     * xxxx x1xx Is for Colliding with Player Boundaries. This is given to players that want to collide with Player Boundaries.
     * xxxx 1xxx Is also for Colliding with Player Boundaries. This is given to Projectiles that want to collide with Player Boundaries.
     * Examples
     * xxxx 11xx, is given to Player Boundaries so that they can differentiate between Player and Projectile Collisions
     * xxxx xx11, is given to Projectiles meaning they collide with Projectile Boundaries and Players
     * xxxx x1x1, is given to Players meaning they collide with PlayerBoundaries and Projectiles
     * xxxx 1xx1, is given to All Projectiles that collide with Player Boundaries AND with Players
     *      The colliion detection system conditionally checks based on unique Components related to the type of projectile it is colliding with
     *      Each projectile with unique Player Boundary collisions are given a "PlayerCollisionWithPlayerBoundaryComponent" that stores a string
     *      detailing which type of unique projectile it is. This string is handled and implemented in the Collision Detection System and
     *      Colliison Listener.
     * xxxx 1xxx, is given to Projectiles that only collide with Player Boundaries. It is worth noting that projectiles xxxx 1xx1 and xxxx 1xxx
     *      projectiles are given the same "PlayerCollisionWithPlayerBoundaryComponent" and thus must be checked in the same section of the 
     *      Collision Detection System and Collision Listener.
     */
    public CollisionComponent(float collisionRadius, float width, byte mask)
    {
        this.collisionRadius = collisionRadius;
        this.width = collisionRadius;
        this.mask = mask;
    }
}

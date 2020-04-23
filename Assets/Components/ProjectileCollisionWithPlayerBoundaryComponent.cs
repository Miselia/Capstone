using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

public struct ProjectileCollisionWithPlayerBoundaryComponent : IComponentData
{
    /* Cases are based on each unique type of collision reaction between projectiles and Player Boundaries.
     * This should just be an enum but Keith isn't particularly concerned with doing that at the moment.
     * 
     * Each respective value can be found in the Constants file under "Unique Projectile x Player Boundary Collision IDs"
     */
    public int caseInt;

    public ProjectileCollisionWithPlayerBoundaryComponent(int caseNum)
    {
        caseInt = caseNum;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ManaSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        /*
        Entities.ForEach(( ref PlayerComp p) =>
        {
            p.mana = p.mana + 1;
            if (p.mana > Constants.PlayerMaximumMana) p.mana = Constants.PlayerMaximumMana;
        });
        */
    }
}

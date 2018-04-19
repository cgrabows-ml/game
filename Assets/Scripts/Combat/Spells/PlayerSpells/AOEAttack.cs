using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : DamageSpell
{


    public AOEAttack(Character caster)
        : base(caster, baseCooldown: 15, baseDamage: 3, animationKey: "Use2",
            triggersGCD: true, target: "aoe", GCDRespect: true, delay: .5f)
    {

    }
}

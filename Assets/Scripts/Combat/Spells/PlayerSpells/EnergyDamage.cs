using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDamage : DamageSpell
{

    Hero hero;
    private int damagePerEnergy = 1;
    Boolean toReset = false;

    public EnergyDamage(Character caster)
        : base(caster, baseCooldown: 0, baseDamage: 0, animationKey: "Use2",
            triggersGCD: true, target: "front", GCDRespect: true, delay: .5f)
    {
        hero = (Hero)caster;

    }


    public override void ReduceCooldown()
    {
        base.ReduceCooldown();
        if (GetCooldown() <= 0 && !(hero.GetEnergy() > 0))
        {
            toReset = true;
            gameController.castCovers[index].localScale = new Vector3(1,1,0);
        }
        if(toReset && GetCooldown() <= 0 && (hero.GetEnergy() > 0))
        {
            toReset = false;
            gameController.castCovers[index].localScale = new Vector3(0, 0, 0);
        }
    }

    public override bool isCastable()
    {
        return base.isCastable() && hero.GetEnergy() > 0;
    }

    public override void Cast()
    {
        int energy = hero.GetEnergy();
        baseDamage = damagePerEnergy * energy;
        hero.LoseEnergy(energy);
        base.Cast();

        Transform prefab = (Transform)Resources.Load(
            "lobproj", typeof(Transform));
        Transform projectile = MonoBehaviour.Instantiate(prefab);
        Vector3 projectileOffset = new Vector2(0, .5f);
        projectile.position = caster.instances[0].position + projectileOffset;
        new Lob(GetTargets()[0].instances[0].position + projectileOffset, projectile,
            delay);
    }
}

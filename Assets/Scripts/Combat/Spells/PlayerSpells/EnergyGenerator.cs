using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : HeroSpell
{
    private float baseDamage = 1;
    private int energyGained = 1;

    public EnergyGenerator(Hero hero)
        : base(hero, baseCooldown: 1, animationKey: "Use2",
            triggersGCD: true, GCDRespect: true, delay:0, canBeEmpowered: false)
    {

    }

    protected override void BasicCast()
    {
        hero.GainEnergy(energyGained);
        Enemy target = gameController.stage.getActiveEnemies()[0];
        CombatUtils.DealDamage(caster, target, baseDamage);
    }

    protected override void EmpoweredCast()
    {
        // can't be empowered
    }
}

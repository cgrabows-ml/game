using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDamage : HeroSpell
{

    private int baseDamage = 5;

    public BigDamage(Hero hero)
        : base(hero, baseCooldown: 8, animationKey: "Use2",
            triggersGCD: true, GCDRespect: true, delay: .5f)
    {

    }

    protected override void BasicCast()
    {
        List<Enemy> enemies = gameController.stage.getActiveEnemies();
        Enemy target = enemies[0];
        CombatUtils.DealDamage(hero, target, baseDamage);
    }

    protected override void EmpoweredCast()
    {
        List<Enemy> enemies = gameController.stage.getActiveEnemies();
        List<Character> targets = enemies.ConvertAll<Character>(x => x);
        CombatUtils.DealDamage(hero, targets, baseDamage);
    }
}

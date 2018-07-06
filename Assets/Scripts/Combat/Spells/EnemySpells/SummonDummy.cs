using System;
using UnityEngine;

public class SummonDummy : SummonEnemy, IDeathObserver
{
    public SummonDummy(Character caster)
        : base(caster)
    {
        baseCooldown = 10;
        maxSummons = 4;
    }

    public override Enemy getEnemy()
    {
        return new Dummy();
    }

    public override void CastEffect()
    {
        foreach(Enemy summon in summons)
        {
            summon.TakeDamage(summon.health - summon.maxHealth,
                caster);
        }
        while(summonsAlive < maxSummons)
        {
            base.CastEffect();
        }
    }
}
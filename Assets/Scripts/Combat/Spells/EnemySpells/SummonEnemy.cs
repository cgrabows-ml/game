using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class SummonEnemy : Spell, IDeathObserver
{

    private Stage stage;
    protected int summonsAlive = 0;
    protected int maxSummons;
    public List<Enemy> summons = new List<Enemy>();

    public SummonEnemy(Character caster, int maxSummons = 3)
        : base(caster: caster, baseCooldown: 5, animationKey:"Use2")
    {
        stage = gameController.stage;
        this.maxSummons = maxSummons;
    }

    public abstract Enemy getEnemy();

    public override void ReduceCooldown()
    {
        if (summonsAlive < maxSummons)
        {
            SetCooldown(Math.Max(0, GetCooldown() - Time.deltaTime));
        }
    }

    public void DeathUpdate(Character character)
    {
        summonsAlive -= 1;
        summons.Remove((Enemy)character);
    }

    public override Boolean isCastable()
    {
        return base.isCastable() && summonsAlive < maxSummons;
    }

    public override void CastEffect()
    {
        int casterIndex = stage.enemies.IndexOf((Enemy)caster);
        Enemy summon = getEnemy();
        stage.AddEnemyAtIndex(summon, casterIndex);
        summon.RegisterDeathObserver(this);
        summons.Add(summon);
        summonsAlive += 1;
    }
}
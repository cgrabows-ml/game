using System;
using UnityEngine;

public abstract class SummonEnemy : Spell, IDeathObserver
{

    private Stage stage;
    private int summonsAlive = 0;
    private int maxSummons;

    public SummonEnemy(Character caster, int maxSummons = 3)
        : base(caster, 5, "Use2")
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
        MonoBehaviour.print(summonsAlive);

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
        summonsAlive += 1;
    }
}
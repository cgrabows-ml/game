using System;
using UnityEngine;

public class SummonSkeleton : Spell, IDeathObserver
{

    private Stage stage;
    private int skeletonsAlive = 0;
    private int maxSkeletons = 3;

    public SummonSkeleton(Character caster)
        :base(caster, 5, "Use2")
    {
        stage = gameController.stage;

    }

    public override void ReduceCooldown()
    {
        if (skeletonsAlive < maxSkeletons)
        {
            SetCooldown(Math.Max(0, GetCooldown() - Time.deltaTime));
        }
    }

    public void DeathUpdate(Character character)
    {
        skeletonsAlive -= 1;
        MonoBehaviour.print(skeletonsAlive);

    }

    public override Boolean isCastable()
    {
        return base.isCastable() && skeletonsAlive < maxSkeletons;
    }

    public override void Cast()
    {
        base.Cast();
        int casterIndex = stage.enemies.IndexOf((Enemy)caster);
        Enemy skeleton = new Skeleton();
        stage.AddEnemyAtIndex(skeleton, casterIndex);
        skeleton.RegisterDeathObserver(this);
        skeletonsAlive += 1;
    }
}
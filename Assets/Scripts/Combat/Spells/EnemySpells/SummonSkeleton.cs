using System;
using UnityEngine;

public class SummonSkeleton : Spell, IDeathObserver
{

    private Stage stage;
    private int skeletonsAlive = 0;
    private int maxSkeletons = 3;

    public SummonSkeleton()
        :base(5, "Use2")
    {
        stage = gameController.stage;

    }

    public void DeathUpdate(Character character)
    {
        skeletonsAlive -= 1;
    }

    public override Boolean isCastable(Character caster)
    {
        return base.isCastable(caster) && skeletonsAlive < maxSkeletons;
    }

    public override void Cast(Character caster)
    {
        base.Cast(caster);
        int casterIndex = stage.enemies.IndexOf((Enemy)caster);
        Enemy skeleton = new Skeleton();
        stage.AddEnemyAtIndex(skeleton, casterIndex);
        skeleton.RegisterDeathObserver(this);
        skeletonsAlive += 1;
    }
}
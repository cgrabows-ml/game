using System;
using UnityEngine;

public class SummonSkeleton : SummonEnemy, IDeathObserver
{
    public SummonSkeleton(Character caster)
        :base(caster)
    {

    }

    public override Enemy getEnemy()
    {
        return new Skeleton();
    }
}
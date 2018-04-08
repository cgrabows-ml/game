using System;
using UnityEngine;

public class SummonCrystal : Spell
{

    private Stage stage;

    public SummonCrystal()
        : base(20, "Use2")
    {
        stage = gameController.stage;
    }

    public override void Cast(Character caster)
    {
        base.Cast(caster);
        int casterIndex = stage.enemies.IndexOf((Enemy)caster);
        Enemy crystal = new Crystal();
        stage.AddEnemyAtIndex(crystal, casterIndex);
    }
}
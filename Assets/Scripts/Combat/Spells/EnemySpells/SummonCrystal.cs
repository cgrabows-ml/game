using System;
using UnityEngine;

public class SummonCrystal : Spell
{

    NecromancerBoss boss;
    private Stage stage;

    public SummonCrystal(Character caster)
        : base(caster, 30, "Use2")
    {
        stage = gameController.stage;
        boss = (NecromancerBoss)caster;
        name = "Summon Crystal";
    }

    public override void Cast()
    {
        base.Cast();
        int casterIndex = stage.enemies.IndexOf((Enemy)caster);
        Crystal crystal = new Crystal();
        boss.crystal = crystal;
        crystal.RegisterDeathObserver(boss);
        stage.AddEnemyAtIndex(crystal, casterIndex);
    }
}
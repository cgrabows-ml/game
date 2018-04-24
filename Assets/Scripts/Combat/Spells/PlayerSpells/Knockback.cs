using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Spell
{

    public Knockback(Character caster)
        : base(caster, baseCooldown: 2, animationKey: "Use2")
    {

    }

    public override void Cast()
    {
        base.Cast();
        Stage stage = gameController.stage;
        Enemy enemy = stage.getActiveEnemies()[0];
        stage.enemies.Remove(enemy);
        stage.enemies.Add(enemy);
        stage.MoveEnemies();
    }
}

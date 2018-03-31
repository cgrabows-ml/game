using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Knockback : Spell
{

    public Knockback()
        : base(baseCooldown: 2, animationKey: "Use2")
    {

    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);
        Stage stage = gameController.stage;
        Enemy enemy = stage.getActiveEnemies()[0];
        stage.enemies.Remove(enemy);
        stage.enemies.Add(enemy);
        stage.MoveEnemies();
    }
}

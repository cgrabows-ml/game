﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSpell : Spell {

    protected float baseDamage;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseCooldown"></param>
    /// <param name="baseDamage"></param>
    /// <param name="animationKey"></param>
    /// <param name="triggersGCD"></param>
    /// <param name="target"></param>
    /// <param name="GCDRespect"></param>
    /// <param name="delay"></param>
    public DamageSpell(Character caster, float baseCooldown, float baseDamage,
        String animationKey, Boolean triggersGCD = true, String target = "front",
        Boolean GCDRespect = true, float delay = 0)
        : base(caster, baseCooldown, animationKey, triggersGCD, GCDRespect, delay)
    {
        this.baseDamage = baseDamage;
        this.target = target;
    }

    public override void CastEffect()
    {
        CombatUtils.DamageAfterTime(caster, GetTargets(), baseDamage, delay);
    }

    public virtual List<Character> GetTargets()
    {
        List<Enemy> enemies = gameController.stage.getActiveEnemies();
        if (target == "player")
        {
            return new List<Character>() { gameController.stage.hero };
        }
        else if (target == "front")
        {
            return new List<Character>() { enemies[0] };
        }
        else if (target == "aoe")
        {
            List<Character> characters = new List<Character>();
            foreach(Enemy enemy in enemies)
            {
                characters.Add(enemy);
            }
            return characters;
        }
        else if (target == "back")
        {
            return new List<Character>() {
               enemies[enemies.Count - 1]
            };
        }
        else
        {
            MonoBehaviour.print("Target not valid for damage spell.");
            return new List<Character>(); //should instead raise error
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSpell : Spell {

    private float baseDamage;

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
    public DamageSpell(float baseCooldown, float baseDamage, String animationKey,
        Boolean triggersGCD = true, String target = "front", Boolean GCDRespect = true, float delay = 0)
        : base(baseCooldown, animationKey, triggersGCD, GCDRespect, delay)
    {
        this.baseDamage = baseDamage;
        this.target = target;
    }

    public override void Cast(Character owner)
    {
        IEnumerator coroutine = DamageAfterTime(delay, owner);
        gameController.StartCoroutine(coroutine);
    }

    public void DealDamage(float damage)
    {
        List<Character> targets = GetTargets();
        targets.ForEach(target => target.TakeDamage(damage));
    }

    public List<Character> GetTargets()
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

    IEnumerator DamageAfterTime(float time, Character owner)
    {
        int startNum = gameController.stage.numEncounter;
        float finalDamage = owner.GetDamage(baseDamage);
        float startTime = 0;
        base.Cast(owner);
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if(startNum == gameController.stage.numEncounter)
        {
            DealDamage(finalDamage);
        }
    }

}

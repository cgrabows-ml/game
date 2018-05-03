using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealLife : HeroSpell
{

    private float baseDamage = 3;
    private float empoweredDamage = 6;

    public StealLife(Hero hero)
        : base(hero, baseCooldown: 5, animationKey: "Use2",
            triggersGCD: true, GCDRespect: true, delay: .5f)
    {
        name = "Steal Life";

    }

    protected override void EmpoweredCast()
    {
        stealLife(empoweredDamage);
    }

    private void stealLife(float damage)
    {
        Enemy target = gameController.stage.getActiveEnemies()[0];
        float damageDealt = CombatUtils.DealDamage(hero, target, damage);
        if (target.health <= 0)
        {
            SetCooldown(0);
        }
        IEnumerator coroutine = HealAfterTime(delay, damageDealt, caster);
        gameController.StartCoroutine(coroutine);
    }

    protected override void BasicCast()
    {
        stealLife(baseDamage);
    }

    IEnumerator HealAfterTime(float time, float damageDealt, Character owner)
    {
        int startNum = numEncounter;
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if (startNum == numEncounter)
        {
            gameController.hero.TakeDamage(-damageDealt, owner);
        }
    }
}
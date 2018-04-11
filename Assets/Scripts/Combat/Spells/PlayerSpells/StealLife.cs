using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealLife : Spell
{

    private float baseDamage = 3;

    public StealLife(Character caster)
        : base(caster, baseCooldown: 5, animationKey: "Use2",
            triggersGCD: true, GCDRespect: true, delay: .5f)
    {

    }

    public override void Cast()
    {
        base.Cast();
        Enemy target = gameController.stage.getActiveEnemies()[0];
        float damageDealt = target.TakeDamage(caster.GetDamage(baseDamage), caster);
        if (target.health <= 0)
        {
            SetCooldown(0);
        }
        IEnumerator coroutine = HealAfterTime(delay, damageDealt, caster);
        gameController.StartCoroutine(coroutine);

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
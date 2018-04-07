using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StealLife : Spell
{

    private float baseDamage = 3;

    public StealLife()
        : base(baseCooldown: 6, animationKey: "Use2",
            triggersGCD: true, GCDRespect: true, delay: .5f)
    {

    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);
        Enemy target = gameController.stage.getActiveEnemies()[0];
        float damageDealt = target.TakeDamage(owner.GetDamage(baseDamage));
        if (target.health <= 0)
        {
            cooldown = 0;
        }
        IEnumerator coroutine = HealAfterTime(delay, damageDealt);
        gameController.StartCoroutine(coroutine);

    }

    IEnumerator HealAfterTime(float time, float damageDealt)
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
            gameController.hero.TakeDamage(-damageDealt);
        }
    }
}
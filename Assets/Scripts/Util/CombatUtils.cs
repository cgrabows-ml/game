using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUtils
{
    public static float DealDamage(Character source, Character target, float baseDamage)
    {
        float outgoingDamage = source.GetDamage(baseDamage);
        float damageTaken = target.TakeDamage(outgoingDamage, source);
        return damageTaken;
    }

    public static float DealDamage(Character source, List<Character> targets, float baseDamage)
    {
        float totalDamage = 0;
        foreach(Character target in targets)
        {
            totalDamage += DealDamage(source, target, baseDamage);
        }
        return totalDamage;
    }

    public static void DamageAfterTime(Character source, Character target, 
        float baseDamage, float time)
    {
        float damage = source.GetDamage(baseDamage);
        IEnumerator coroutine = DamageAfterTimeCoroutine(source, target, damage, time);
        GameController.gameController.StartCoroutine(coroutine);
    }

    public static void DamageAfterTime(Character source, List<Character> targets,
    float baseDamage, float time)
    {
        foreach (Character target in targets)
        {
            DamageAfterTime(source, target, baseDamage, time);
        }
    }

    static IEnumerator DamageAfterTimeCoroutine(Character source, Character target,
        float damage, float time)
    {
        int startNum = GameController.gameController.stage.numEncounter;
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if (startNum == GameController.gameController.stage.numEncounter)
        {
            target.TakeDamage(damage, source);
        }
    }
}

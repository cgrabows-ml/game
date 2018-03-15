using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSpell : Spell {

    private float baseDamage;

    public DamageSpell(float baseCooldown, float baseDamage, String animationKey, Boolean triggersGCD = true, String target = "front", Boolean GCDRespect = true, float delay = 0)
        : base(baseCooldown, animationKey, triggersGCD, target, GCDRespect, delay)
    {
        this.baseDamage = baseDamage;
    }

    public override void Cast(Character owner)
    {
        IEnumerator coroutine = DamageAfterTime(delay, owner);
        playerController.StartCoroutine(coroutine);
    }

    public void DealDamage(float damage)
    {
        List<Character> targets = GetTargets();
        targets.ForEach(target => target.TakeDamage(damage));
    }

    IEnumerator DamageAfterTime(float time, Character owner)
    {
        int startNum = numEncounter;
        float finalDamage = owner.GetDamage(baseDamage);
        float startTime = 0;
        base.Cast(owner);
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if(startNum == numEncounter)
        {
            DealDamage(finalDamage);
        }
    }

}

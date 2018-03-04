using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSpell : Spell {

    private float baseDamage;

    public DamageSpell(float baseCooldown, float baseDamage, String animationKey, Boolean triggersGCD = true, String target = "front", Boolean GCDRespect = true)
        : base(baseCooldown, animationKey, triggersGCD, target, GCDRespect)
    {
        this.baseDamage = baseDamage;
    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);
        DealDamage(owner.GetDamage(baseDamage));
    }

    public void DealDamage(float damage)
    {
        List<Character> targets = GetTargets();
        targets.ForEach(target => target.TakeDamage(damage));
    }

}

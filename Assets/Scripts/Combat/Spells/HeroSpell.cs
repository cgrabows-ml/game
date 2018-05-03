using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class HeroSpell: Spell
{
    protected Hero hero;
    private Boolean canBeEmpowered;

    public HeroSpell(Hero hero, float baseCooldown,
        String animationKey, Boolean triggersGCD = true, Boolean GCDRespect = true,
        float delay = 0, Boolean canBeEmpowered = true)
        : base(hero, baseCooldown, animationKey, triggersGCD, GCDRespect, delay)
    {
        this.hero = hero;
        this.canBeEmpowered = canBeEmpowered;
    }

    public override void CastEffect()
    {
        if (canBeEmpowered && hero.GetEnergy() == hero.maxEnergy)
        {
            hero.LoseAllEnergy();
            EmpoweredCast();
        }
        else
        {
            BasicCast();
        }
    }

    protected abstract void EmpoweredCast();
    protected abstract void BasicCast();



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpowerBuff : Buff, SpellCastObserver {

    private int multiplierDiff = 1;
    private int maxSpellCasts = 2;
    
    public EmpowerBuff(Character owner)
        :base("Empowered", owner, 5)
    {
    }

    public void SpellCastUpdate(Spell spell, Character caster)
    {
        if (caster == owner)
        {
            maxSpellCasts -= 1;
            if (maxSpellCasts <= 0)
            {
                RemoveBuff();
            }
        }
        else
        {
            //Throw Exception
        }
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        owner.outMultiplier += multiplierDiff;
        owner.RegisterCastListener(this);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        owner.outMultiplier -= multiplierDiff;
        owner.UnregisterCastListener(this);
    }
}

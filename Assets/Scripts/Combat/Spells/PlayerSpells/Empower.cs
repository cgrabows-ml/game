using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empower : Spell {

    public Buff empowerBuff;

    public Empower(Character caster)
        :base(caster, 10, "Use4", false, GCDRespect: false)
    {
    }

    public override void CastEffect()
    {
        empowerBuff = new EmpowerBuff(caster);
        empowerBuff.ApplyBuff();
    }

}

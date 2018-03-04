using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empower : Spell {

    public Buff empowerBuff;

    public Empower()
        :base(10, "Use4", false, GCDRespect: false)
    {
    }
    public override void Cast(Character caster)
    {
        base.Cast(caster);
        empowerBuff = new EmpowerBuff(caster);
    }

}

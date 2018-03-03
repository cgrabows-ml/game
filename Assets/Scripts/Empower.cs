using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empower : Spell {

    public Buff empowerBuff;

    public Empower()
        :base(10, 0, "Use4", false, GCDRespect: false)
    {
        empowerBuff = new Buff(2, 0, "Empower", "numSpells", numSpells : 2);
    }
    public override void Cast(Character owner)
    {
        base.Cast(owner);
        owner.buffs.Add(empowerBuff);
        owner.out_multiplier *= 2;
    }

}

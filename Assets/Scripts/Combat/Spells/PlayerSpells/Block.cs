using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Spell
{

    public Buff blockBuff;

    public Block()
        : base(10, "Use4", false, GCDRespect: false)
    {
    }
    public override void Cast(Character caster)
    {
        base.Cast(caster);
        blockBuff = new BlockBuff(caster);
    }

}

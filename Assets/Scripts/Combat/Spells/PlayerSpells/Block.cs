using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Spell
{

    public Buff blockBuff;

    public Block(Character caster)
        : base(caster, 10, "Use4", false, GCDRespect: false)
    {
    }
    public override void CastEffect()
    {
        blockBuff = new BlockBuff(caster);
        blockBuff.ApplyBuff();
    }

}

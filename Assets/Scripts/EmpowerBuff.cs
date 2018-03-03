using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpowerBuff : Buff {

    private int multiplierDiff = 1;
    
    public EmpowerBuff(Character owner)
        :base("Empowered", owner, 5)
    {
    }

    public override void ApplyBuff()
    {
        owner.out_multiplier += multiplierDiff;
    }

    public override void RemoveBuff()
    {
        owner.out_multiplier -= multiplierDiff;
    }
}

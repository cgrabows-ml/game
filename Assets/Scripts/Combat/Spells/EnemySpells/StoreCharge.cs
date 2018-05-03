using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCharge : Spell
{
    private float distanceFromCenter;
    private float distanceGrowth = 0;
    private Vector3 chargeOffsetCenter = new Vector2(0, 4.5f);
    NecromancerBoss boss;

    private int chargesPerCast;

    public StoreCharge(Character caster, int chargesPerCast = 1)
        : base(caster, baseCooldown: 6f, animationKey: "Use2")
    {
        this.chargesPerCast = chargesPerCast;
        boss = (NecromancerBoss)caster;
        distanceFromCenter = .5f + distanceGrowth * boss.maxCharges;
        name = "Store Charge";
    }

    public override Boolean isCastable()
    {
        return base.isCastable() && boss.charges.Count < boss.maxCharges;
    }

    public override void CastEffect()
    {
        for(int i = 0; i < chargesPerCast; i++)
        {
            Transform prefab = (Transform)Resources.Load(
                "fireball", typeof(Transform));
            Transform charge = MonoBehaviour.Instantiate(prefab);

            int numCharges = boss.charges.Count;

            Vector3 chargeCenter = chargeOffsetCenter + caster.instances[0].position;

            float angle = (2 * Mathf.PI / boss.maxCharges) * (numCharges + 1);
            Vector3 chargeOffset = new Vector3(distanceFromCenter * Mathf.Cos(angle),
                distanceFromCenter * Mathf.Sin(angle));
            //MonoBehaviour.print(chargeOffset.x + ", " +  chargeOffset.y);
            charge.position = chargeCenter + chargeOffset;
            boss.StoreCharge(charge);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoreCharge : Spell
{
    private float distanceFromCenter = .5f;
    private Vector3 chargeOffsetCenter = new Vector2(0, 4.5f);
    NecromancerBoss boss;

    public StoreCharge(Character caster)
        : base(caster, baseCooldown: 6f, animationKey: "Use2")
    {
        boss = (NecromancerBoss)caster;
        name = "Store Charge";
    }

    public override Boolean isCastable()
    {
        return base.isCastable() && boss.charges.Count < boss.maxCharges;
    }

    public override void Cast()
    {
        base.Cast();
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/fireball.prefab", typeof(Transform));
        Transform charge = MonoBehaviour.Instantiate(prefab);

        int numCharges = boss.charges.Count;

        Vector3 chargeCenter = chargeOffsetCenter + caster.instances[0].position;

        float angle = (2*Mathf.PI/boss.maxCharges) * (numCharges + 1);
        Vector3 chargeOffset = new Vector3(distanceFromCenter * Mathf.Cos(angle),
            distanceFromCenter*Mathf.Sin(angle));
        //MonoBehaviour.print(chargeOffset.x + ", " +  chargeOffset.y);
        charge.position = chargeCenter + chargeOffset;
        boss.StoreCharge(charge);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoreCharge : Spell
{
    private float distanceFromCenter = .5f;
    private Vector3 chargeOffsetCenter = new Vector2(0, 4.5f);


    public StoreCharge()
        : base(baseCooldown: 3f, animationKey: "Use2")
    {
    }

    public override Boolean isCastable(Character caster)
    {
        NecromancerBoss boss = (NecromancerBoss)caster;
        return base.isCastable(caster) && boss.charges.Count < boss.maxCharges;
    }

    public override void Cast(Character caster)
    {
        base.Cast(caster);
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/fireball.prefab", typeof(Transform));
        Transform charge = MonoBehaviour.Instantiate(prefab);
        NecromancerBoss boss = (NecromancerBoss)caster;

        int numCharges = boss.charges.Count;

        Vector3 chargeCenter = chargeOffsetCenter + caster.instances[0].position;

        float angle = (2*Mathf.PI/boss.maxCharges) * (numCharges + 1);
        Vector3 chargeOffset = new Vector3(distanceFromCenter * Mathf.Cos(angle),
            distanceFromCenter*Mathf.Sin(angle));
        MonoBehaviour.print(chargeOffset.x + ", " +  chargeOffset.y);
        charge.position = chargeCenter + chargeOffset;
        boss.charges.Add(charge);
    }

}

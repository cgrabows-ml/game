﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnergyDamage : DamageSpell
{

    private int damagePerEnergy = 1;

    public EnergyDamage()
        : base(baseCooldown: 0, baseDamage: 0, animationKey: "Use2",
            triggersGCD: true, target: "front", GCDRespect: true, delay: .5f)
    {

    }

    public override bool isCastable(Character caster)
    {
        Hero hero = (Hero) caster;
        return base.isCastable(caster) && hero.GetEnergy() > 0;
    }

    public override void Cast(Character owner)
    {
        Hero hero = (Hero) owner;
        int energy = hero.GetEnergy();
        baseDamage = damagePerEnergy * energy;
        hero.LoseEnergy(energy);
        base.Cast(owner);

        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/lobproj.prefab", typeof(Transform));
        Transform projectile = MonoBehaviour.Instantiate(prefab);
        Vector3 projectileOffset = new Vector2(0, .5f);
        projectile.position = owner.instances[0].position + projectileOffset;
        new Lob(GetTargets()[0].instances[0].position + projectileOffset, projectile,
            delay);
    }
}

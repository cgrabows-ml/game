using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fireball : DamageSpell
{

    public Transform fireball;

    public Fireball()
        : base(6, 2, "Use2", true, "front", true, 4)
    {

    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);

        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/lobproj.prefab", typeof(Transform));

        new Lob(owner.instances[0].position + new Vector3(0, .5f, 0), GetTargets()[0].instances[0].position + new Vector3(0, .5f, 0), prefab, delay);

    }


}
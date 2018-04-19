using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EmpowerBuff : Buff, SpellCastObserver {

    private int multiplierDiff = 1;
    private int maxSpellCasts = 2;
    private Transform empowerBuff;
    
    public EmpowerBuff(Character owner)
        :base("Empowered", owner, 5)
    {
    }

    public void SpellCastUpdate(Spell spell, Character caster)
    {
        if (caster == owner)
        {
            maxSpellCasts -= 1;
            if (maxSpellCasts <= 0)
            {
                RemoveBuff();
            }
        }
        else
        {
            //Throw Exception
        }
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        owner.outMultiplier += multiplierDiff;
        owner.RegisterCastListener(this);

        //Create game object for Empower graphic
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/PWS.prefab", typeof(Transform));
        empowerBuff = MonoBehaviour.Instantiate(prefab);
        Transform ownerPrefab = owner.instances[0];
        empowerBuff.position = ownerPrefab.position + new Vector3(0, 1.8f);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        owner.outMultiplier -= multiplierDiff;
        owner.UnregisterCastListener(this);
        MonoBehaviour.Destroy(empowerBuff.gameObject);
    }
}

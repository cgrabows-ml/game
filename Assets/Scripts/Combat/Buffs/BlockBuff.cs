using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBuff : Buff
{
    private float multiplierDiff = .5f;
    private Transform empowerBuff;


    public BlockBuff(Character owner)
        :base("Blocking", owner, duration: 3)
    {
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        owner.inMultiplier -= multiplierDiff;

        //Create game object for Empower graphic
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/PWS.prefab", typeof(Transform));
        empowerBuff = MonoBehaviour.Instantiate(prefab);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        owner.inMultiplier += multiplierDiff;
        MonoBehaviour.Destroy(empowerBuff.gameObject);
    }
}

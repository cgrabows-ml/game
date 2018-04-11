using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrystalBuff : Buff
{
    private float multiplierDiff = 1f;
    private Transform crystalBuff;


    public CrystalBuff(Character owner)
        : base("Crystal Power", owner, duration: 10)
    {
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        owner.outMultiplier += multiplierDiff;

        //Create game object for Empower graphic
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/PWS.prefab", typeof(Transform));
        crystalBuff = MonoBehaviour.Instantiate(prefab);
        crystalBuff.position = owner.instances[0].position + new Vector3(0, 1f, 0);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        owner.outMultiplier -= multiplierDiff;
        MonoBehaviour.Destroy(crystalBuff.gameObject);
    }
}

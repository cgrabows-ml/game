using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBuff : Buff
{
    private float multiplierDiff = 1f;
    private Transform crystalBuff;
    private Vector3 startPos;


    public CrystalBuff(Character owner, Vector3 startPos)
        : base("Crystal Power", owner, duration: 10)
    {
        this.startPos = startPos;
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        owner.outMultiplier += multiplierDiff;

        //Create game object for Empower graphic
        Vector3 projectileOffset = new Vector3(0, 0,0);
        float delay = 2;
        Transform prefab = (Transform)Resources.Load("PWS", typeof(Transform));
        crystalBuff = MonoBehaviour.Instantiate(prefab);

        crystalBuff.localPosition = startPos;
        //new Projectile(owner.instances[0].localPosition + projectileOffset, crystalBuff,
        //    delay);
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        owner.outMultiplier -= multiplierDiff;
        MonoBehaviour.Destroy(crystalBuff.gameObject);
    }
}

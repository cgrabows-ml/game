using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBuff : Buff
{
    private float multiplierDiff = 1f;
    private Transform crystalBuff;
    private Vector3 startPos;
    private float delay;
    private Transform prefab;
    private Vector3 projectileOffset = new Vector3(0, .4f, 0);


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
        delay = 2;
        prefab = (Transform)Resources.Load("PWS", typeof(Transform));
        crystalBuff = MonoBehaviour.Instantiate(prefab);

        crystalBuff.localPosition = startPos;
        new Projectile(owner.sprite.localPosition + projectileOffset, crystalBuff,
            delay);

        IEnumerator coroutine = makeAfterDelay();
        GameController.gameController.StartCoroutine(coroutine);

    }

    IEnumerator makeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        crystalBuff = MonoBehaviour.Instantiate(prefab);
        crystalBuff.localPosition = owner.sprite.localPosition + projectileOffset;
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        owner.outMultiplier -= multiplierDiff;
        MonoBehaviour.Destroy(crystalBuff.gameObject);
    }
}

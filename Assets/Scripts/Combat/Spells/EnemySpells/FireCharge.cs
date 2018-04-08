using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireCharges : DamageSpell
{

    private float timeBetweenShots = 1f;

    public FireCharges()
        : base(baseCooldown: 10, baseDamage: 3, animationKey: "Use2",
            triggersGCD: true, target: "player", GCDRespect: true, delay: .5f)
    {

    }

    public override bool isCastable(Character caster)
    {
        NecromancerBoss boss = (NecromancerBoss)caster;
        return base.isCastable(caster) && boss.charges.Count == boss.maxCharges;
    }

    public override void Cast(Character caster)
    {
        base.Cast(caster);

        NecromancerBoss boss = (NecromancerBoss)caster;

        Transform projectile = boss.charges[0];
        boss.charges.Remove(projectile);
        Vector3 projectileOffset = new Vector2(0, .5f);
        new Projectile(GetTargets()[0].instances[0].position + projectileOffset, projectile,
            delay);
        gameController.StartCoroutine(CastAgain(boss));
    }

    IEnumerator CastAgain(NecromancerBoss boss)
    {
        if (boss.charges.Count != 0)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            Cast(boss);
        }
    }
}
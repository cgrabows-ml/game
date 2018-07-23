using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCharges : DamageSpell
{

    private NecromancerBoss boss;
    private float timeBetweenShots = 1f;

    public FireCharges(Character caster)
        : base(caster: caster, baseCooldown: 10, baseDamage: 3, animationKey: "skill_3",
            triggersGCD: true, target: "player", GCDRespect: true, delay: .5f)
    {
        boss = (NecromancerBoss)caster;
        name = "Fire Charges";
    }

    public override bool isCastable()
    {
        return base.isCastable() && boss.charges.Count == boss.maxCharges;
    }

    public override void CastEffect()
    {
        base.CastEffect();
        Transform projectile = boss.SpendCharge();
        Vector3 projectileOffset = new Vector2(0, .5f);
        new Projectile(GetTargets()[0].sprite.position + projectileOffset, projectile,
            delay);
        gameController.StartCoroutine(CastAgain());
    }

    IEnumerator CastAgain()
    {
        if (boss.charges.Count != 0 && boss.health > 0)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            Cast();
        }
    }
}
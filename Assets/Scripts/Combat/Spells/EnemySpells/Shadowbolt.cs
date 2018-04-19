using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBolt : DamageSpell
{
    NecromancerBoss boss;


    public ShadowBolt(Character caster)
        : base(caster, baseCooldown: 4, baseDamage: 4, animationKey: "Use2",
            triggersGCD: true, target: "player", GCDRespect: true, delay: .5f)
    {
        boss = (NecromancerBoss)caster;
        name = "Shadowbolt";
    }

    public override List<Character> GetTargets()
    {
        Character target = gameController.stage.hero;
        List<Enemy> enemies = gameController.stage.getActiveEnemies();
        if(boss.crystal != null)
        {
            target = boss.crystal;
        }
        return new List<Character>() { target };
    }

    public override void Cast()
    {
        base.Cast();

        Transform prefab = (Transform)Resources.Load(
            "lobproj", typeof(Transform));
        Transform projectile = MonoBehaviour.Instantiate(prefab);
        Vector3 projectileOffset = new Vector2(0, .5f);
        projectile.position = caster.instances[0].position + projectileOffset;
        new Lob(GetTargets()[0].instances[0].position + projectileOffset, projectile,
            delay);
    }
}


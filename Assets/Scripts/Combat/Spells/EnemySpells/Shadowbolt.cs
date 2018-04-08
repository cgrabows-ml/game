using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShadowBolt : DamageSpell
{


    public ShadowBolt()
        : base(baseCooldown: 4, baseDamage: 4, animationKey: "Use2",
            triggersGCD: true, target: "player", GCDRespect: true, delay: .5f)
    {

    }

    public override List<Character> GetTargets()
    {
        Character target = gameController.stage.hero;
        List<Enemy> enemies = gameController.stage.getActiveEnemies();

        foreach (Enemy enemy in enemies)
        {
            if (enemy.name == "crystal")
            {
                target = enemy;
            }
        }
        return new List<Character>() { target };
    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);
        NecromancerBoss  boss = (NecromancerBoss)owner;

        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/lobproj.prefab", typeof(Transform));
        Transform projectile = MonoBehaviour.Instantiate(prefab);
        Vector3 projectileOffset = new Vector2(0, .5f);
        projectile.position = owner.instances[0].position + projectileOffset;
        new Lob(GetTargets()[0].instances[0].position + projectileOffset, projectile,
            delay);
    }
}


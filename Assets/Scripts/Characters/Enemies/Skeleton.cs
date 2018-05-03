using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : Enemy
{
    private static float baseSize = .75f;
    private static float sizeIncreasePerLevel = .25f;
    private static float baseDamage = 1;
    private static float attackCooldown = 5;
    private static float damageIncreasePerLevel = 1;
    private int level;
    private static float baseHealth = 3;
    private static float healthIncreasePerLevel = 3;

    public Skeleton(int level = 1)
        : base("skeleton", "warrior", getTextBox(), getHealth(level), maxGCD: 3)
    {
        this.level = level;
        sizeScale *= baseSize + ((level-1) * sizeIncreasePerLevel);
    }

    private static float getHealth(int level)
    {
        return baseHealth + ((level-1) * healthIncreasePerLevel);
    }

    protected override List<Spell> getSpells()
    {
        float damage = baseDamage + damageIncreasePerLevel * level;
        Spell spell1 = new DamageSpell(this, attackCooldown, damage, "Use1", target: "player");
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)Resources.Load(textBoxPath, typeof(TextMesh));
    }
}

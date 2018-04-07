﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : Enemy
{
    private static float sizeIncreasePerLevel = .25f;
    private static float baseDamage = 1;
    private static float damageIncreasePerLevel = 1;
    private int level;
    private static float baseHealth = 3;
    private static float healthIncreasePerLevel = 3;

    public Skeleton(int level = 1)
        : base("skeleton", "Warrior.prefab", getTextBox(), getHealth(level), maxGCD: 3)
    {
        this.level = level;
        sizeScale *= 1 + ((level-1) * sizeIncreasePerLevel);
    }

    private static float getHealth(int level)
    {
        return baseHealth + ((level-1) * healthIncreasePerLevel);
    }

    protected override List<Spell> getSpells()
    {
        float damage = baseDamage + damageIncreasePerLevel * level;
        Spell spell1 = new DamageSpell(4, damage, "Use1", target: "player");
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "Assets/Prefabs/healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)AssetDatabase.LoadAssetAtPath(textBoxPath, typeof(Transform));
    }
}

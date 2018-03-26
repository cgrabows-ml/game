using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Necromancer : Enemy
{

    public Necromancer()
        : base("necromancer", "Warrior.prefab", getTextBox(), 5, maxGCD: 3)
    {
        this.isFixed = true;
        this.moveTo = new Vector2(5, -2.58f);
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new SummonSkeleton();
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "Assets/Prefabs/healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)AssetDatabase.LoadAssetAtPath(textBoxPath, typeof(Transform));
    }
}

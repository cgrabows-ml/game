using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : Enemy
{

    public Skeleton()
        : base("skeleton", "Warrior.prefab", getTextBox(), 3, maxGCD: 3)
    {

    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new DamageSpell(4, 1, "Use1", target: "player");
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "Assets/Prefabs/healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)AssetDatabase.LoadAssetAtPath(textBoxPath, typeof(Transform));
    }
}

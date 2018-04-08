using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Giant : Enemy {

    public Giant()
        : base("giant", "Giant.prefab", getTextBox(), health: 10, maxGCD: 5)
    {

    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new DamageSpell(10, 5, "Use1", target: "player");
        // Spell spell2 = new DamageSpell(4, 1, "Use1", target: "player");
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "Assets/Prefabs/healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)AssetDatabase.LoadAssetAtPath(textBoxPath, typeof(Transform));
    }
}

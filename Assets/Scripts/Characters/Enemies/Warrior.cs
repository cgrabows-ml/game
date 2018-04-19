using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : Enemy {

    public Warrior()
        : base("warrior", "Warrior", getTextBox(), health: 4, maxGCD: 3)
    {

    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new DamageSpell(this, 4, 1, "Use1", target: "player");
        Spell spell2 = new DamageSpell(this, 4, 1, "Use1", target: "player");
        List<Spell> spells = new List<Spell> { spell1, spell2 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)Resources.Load(textBoxPath, typeof(TextMesh));
    }
}

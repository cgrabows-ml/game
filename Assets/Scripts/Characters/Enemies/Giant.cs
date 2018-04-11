using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Giant : Enemy
{

    public Giant()
        : base("Giant", "warrior", getTextBox(), health: 100, maxGCD: 3)
    {

    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new DamageSpell(this, baseCooldown: 4, baseDamage: 3,
            animationKey:"Use1", target: "player");
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)Resources.Load(textBoxPath, typeof(Transform));
    }
}
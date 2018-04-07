using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerBoss : Enemy
{

    public NecromancerBoss()
        : base("necromancer", "warrior.prefab", getTextBox(), 5, maxGCD: 3)
    {
        this.isFixed = true;
        this.moveTo = new Vector2(4, -2.58f);
        sizeScale = 2.5f;
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new SummonSkeleton();
        //Spell spell2 = new DamageSpell(3, 4, "Use1", target: "player");
        List<Spell> spells = new List<Spell>() { spell1 };
        return spells;
    }   

    private static TextMesh getTextBox()
    {
        string textBoxPath = "Assets/Prefabs/healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)AssetDatabase.LoadAssetAtPath(textBoxPath, typeof(Transform));
    }
}

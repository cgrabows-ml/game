using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerBoss : Enemy
{

    public int maxCharges = 5;
    public List<Transform> charges = new List<Transform>();

    public NecromancerBoss()
        : base("necromancer", "warrior.prefab", getTextBox(), health:100, maxGCD: 1)
    {
        this.isFixed = true;
        this.moveTo = new Vector2(5f, -2.58f);
        sizeScale = 2.5f;
        hasCollision = false;
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new MassSummonSkeleton();
        Spell spell2 = new StoreCharge();
        Spell spell3 = new FireCharges();
        Spell spell4 = new SummonCrystal();
        Spell spell5 = new ShadowBolt();

        List<Spell> spells = new List<Spell>() { spell1, spell2, spell3, spell4, spell5 };
        return spells;
    }   

    private static TextMesh getTextBox()
    {
        string textBoxPath = "Assets/Prefabs/healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)AssetDatabase.LoadAssetAtPath(textBoxPath, typeof(Transform));
    }
}

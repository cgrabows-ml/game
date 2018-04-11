using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerBoss : Enemy, IDeathObserver
{

    //enum behaviors { IntroDPS, DPS, Alternate, SummonSkeles, SummonCrystal, SummonBoth,
    //SuperCharge}
    //private behaviors behavior = behaviors.DPS;
    public int maxCharges = 5;
    public List<Transform> charges = new List<Transform>();
    public Crystal crystal = null;

    private float timeBetweenSummons = 15f;

    private List<Spell> pureDPSSpellbook;
    private List<Spell> alternateSpellbook;
    private List<Spell> summonSkeleSpellbook;
    private List<Spell> summonCrystalSpellbook;
    private List<Spell> summonBothSpellbook;
    private List<Spell> superChargeSpellbook;
    private List<Spell> totalSpellbook;
    private List<Spell> fireChargesSpellbook;

    private List<Spell> currentSpellbook;

    public NecromancerBoss()
        : base("necromancer", "warrior", getTextBox(), health:100, maxGCD: 1)
    {
        this.isFixed = true;
        this.moveTo = new Vector2(5f, -2.58f);
        sizeScale = 2.5f;
        hasCollision = false;
        float initialDPSTime = 5f;
        ChangeSpellbook(summonSkeleSpellbook, initialDPSTime);
        //ChangeSpellbook(pureDPSSpellbook, initialDPSTime + 1.5f);
        //ChangeSpellbook(summonCrystalSpellbook, initialDPSTime + 1.5f + timeBetweenSummons);
    }

    public override void Cast()
    {
        if (currentSpellbook == fireChargesSpellbook && charges.Count == 0)
        {
            currentSpellbook = alternateSpellbook;
            ChangeSpellbook(summonCrystalSpellbook, timeBetweenSummons);
        }
        if (currentSpellbook == summonSkeleSpellbook)
        {
            if (CastIfAble(summonSkeleSpellbook[0]))
            {
                //MonoBehaviour.print("Cast summon skeles");
                ChangeSpellbook(fireChargesSpellbook);
            }
        }
        //Cast Spells
        if (isActive)
        {
            currentSpellbook.ForEach(spell => CastIfAble(spell));
        }
    }

    protected override List<Spell> getSpells()
    {
        Spell fireCharges = new FireCharges(this);
        Spell summonSkeles = new MassSummonSkeleton(this);
        Spell summonCrystal = new SummonCrystal(this);
        Spell storeCharge = new StoreCharge(this);
        Spell shadowBolt = new ShadowBolt(this);
        //
        alternateSpellbook = new List<Spell>() { shadowBolt, storeCharge };
        pureDPSSpellbook = new List<Spell>() { shadowBolt };
        summonSkeleSpellbook = new List<Spell>() { summonSkeles };
        fireChargesSpellbook = new List<Spell>() { fireCharges };
        summonCrystalSpellbook = new List<Spell>() { summonCrystal, shadowBolt };
        summonBothSpellbook = new List<Spell>() { summonSkeles, summonCrystal };
        superChargeSpellbook = new List<Spell>() { shadowBolt };

        totalSpellbook = new List<Spell>(){
            summonSkeles, summonCrystal, fireCharges, shadowBolt, storeCharge};
        currentSpellbook = pureDPSSpellbook;
        return totalSpellbook;
    }

    public void StoreCharge(Transform transform)
    {
        charges.Add(transform);
        if (charges.Count == maxCharges)
        {
            ChangeSpellbook(summonSkeleSpellbook);
        }
    }

    public Transform SpendCharge()
    {
        Transform charge = charges[0];
        charges.Remove(charge);
        if (charges.Count == 0)
        {
            ChangeSpellbook(alternateSpellbook);
        }
        return charge;
    }

    public void DeathUpdate(Character character)
    {
        crystal = null;
        currentSpellbook = alternateSpellbook;
    }

    public void ChangeSpellbook(List<Spell> spellbook, float seconds=0)
    {
        IEnumerator coroutine = 
            ChangeSpellbookAfterDelay(spellbook, seconds);
        gameController.StartCoroutine(coroutine);
    }

    IEnumerator ChangeSpellbookAfterDelay(List<Spell> spellbook, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentSpellbook = spellbook;
        //MonoBehaviour.print("Changed Spellbook");
        foreach(Spell spell in spellbook)
        {
            //MonoBehaviour.print(spell.name);
        }
    }

    private static TextMesh getTextBox()
    {
        string textBoxPath = "healthbar_sprite"; //TODO: read from config or other
        return (TextMesh)Resources.Load(textBoxPath, typeof(TextMesh));
    }
}

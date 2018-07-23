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
    private Boolean firstCrystal = true;

    private int transitionHealthThreshhold = 20;
    private int finalHealthThreshhold = 10;
    private int phase = 1;
    private int phaseTwoMaxCharges = 15;

    private List<Spell> currentSpellbook;

    public NecromancerBoss()
        : base("necromancer", "boss_death", health:100, maxGCD: 1)
    {
        this.spriteType = "special";
        this.deathAnim = "death";
        this.takeDamageAnim = "hit_1";
        this.walkAnim = "walk";
        this.idleAnim = "idle_1";
        this.attack1Anim = "skill_3";
        this.attack2Anim = "skill_3";
        this.attack3Anim = "skill_3";
        this.isFixed = true;
        this.moveTo = new Vector2(4.5f, -2.58f);
        sizeScale = .65f;
        hasCollision = false;
        float initialDPSTime = 5f;
        //Pure DPS -> Summon Skeletons
        ChangeSpellbook(summonSkeleSpellbook, initialDPSTime);
    }

    public override float TakeDamage(float baseDamage, Character source)
    {
        float damage = base.TakeDamage(baseDamage, source);
        if (phase == 1  && health <= transitionHealthThreshhold)
        {
            //Transition to stage 2
            maxCharges = phaseTwoMaxCharges;
            phase = 2;
            foreach (Transform charge in charges)
            {
                MonoBehaviour.Destroy(charge.gameObject);
            }
            charges = new List<Transform>();
            ChangeSpellbook(superChargeSpellbook);
        }
        if (phase == 2 && health <= finalHealthThreshhold)
        {
            //Transition to final burn
            phase = 3;
            ChangeSpellbook(fireChargesSpellbook);
        }
        return damage;
    }

    public override void InstantiateCharacter(Vector2 position)
    {
        base.InstantiateCharacter(position);
        sprite.localRotation = Quaternion.Euler(0, 180, 0);

    }

    public override void Cast()
    {
        if (currentSpellbook == fireChargesSpellbook && charges.Count == 0)
        {
            //Fire Charges -> Alternating DPS
            currentSpellbook = alternateSpellbook;
            if (firstCrystal)
            {
                //Alternating DPS -> Summon Crystal
                ChangeSpellbook(summonCrystalSpellbook, timeBetweenSummons);
                firstCrystal = false;
            }
            else
            {
                //Alternating DPS -> Summon Skeletons -> Summon Crystal
                ChangeSpellbook(summonBothSpellbook, timeBetweenSummons);
            }
        }
        if (currentSpellbook == summonSkeleSpellbook)
        {
            if (CastIfAble(summonSkeleSpellbook[0]))
            {
                //Summon Skeletons -> Fire Charges
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
        Spell weakSummonSkeles = new MassSummonSkeleton(this, levelGainPerCast: 0);
        Spell summonCrystal = new SummonCrystal(this);
        Spell storeCharge = new StoreCharge(this);
        Spell superCharge = new StoreCharge(this, chargesPerCast: phaseTwoMaxCharges);
        Spell shadowBolt = new ShadowBolt(this);
        //
        alternateSpellbook = new List<Spell>() { shadowBolt, storeCharge };
        pureDPSSpellbook = new List<Spell>() { shadowBolt };
        summonSkeleSpellbook = new List<Spell>() { summonSkeles };
        fireChargesSpellbook = new List<Spell>() { fireCharges };
        //Implicitly handles summon crystal -> pure DPS
        summonCrystalSpellbook = new List<Spell>() { summonCrystal, shadowBolt };
        summonBothSpellbook = new List<Spell>() { weakSummonSkeles, summonCrystal, shadowBolt };
        superChargeSpellbook = new List<Spell>() { superCharge, shadowBolt };

        totalSpellbook = new List<Spell>(){
            summonSkeles, summonCrystal, fireCharges, shadowBolt, storeCharge,
            weakSummonSkeles, superCharge };
        currentSpellbook = pureDPSSpellbook;
        return totalSpellbook;
    }

    public void StoreCharge(Transform transform)
    {
        charges.Add(transform);
        if (charges.Count == maxCharges)
        {
            //Alternating -> Summon Skeletons
            ChangeSpellbook(summonSkeleSpellbook);
        }
    }

    public Transform SpendCharge()
    {
        Transform charge = charges[0];
        charges.Remove(charge);
        if (charges.Count == 0)
        {
            //Fire Charges -> Alternating
            ChangeSpellbook(alternateSpellbook);
        }
        return charge;
    }

    public void DeathUpdate(Character character)
    {
        crystal = null;
        //Pure DPS -> Alternating
        if(health > finalHealthThreshhold)
        {
            currentSpellbook = alternateSpellbook;
        }
    }

    public void ChangeSpellbook(List<Spell> spellbook, float seconds=0)
    {
        IEnumerator coroutine = 
            ChangeSpellbookAfterDelay(spellbook, seconds, phase);
        gameController.StartCoroutine(coroutine);
    }

    IEnumerator ChangeSpellbookAfterDelay(List<Spell> spellbook, float seconds, int oldPhase)
    {
        yield return new WaitForSeconds(seconds);
        if (phase == 1 || spellbook == fireChargesSpellbook || spellbook == superChargeSpellbook)
        {
            currentSpellbook = spellbook;
        }
    }
}

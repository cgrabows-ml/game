using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

    public Text heroHealthText;
    public Text enemyHealthText;
    public Text cast1Text;
    public Text cast2Text;
    public Text cast3Text;
    public Text GCDText;
    public Animator hero;
    public Animator enemy;

    private int heroHealth;
    private float enemyHealth;
    private float GCD;
    private float multiplier;
    private float addititve;
    private float enemyCastCD;
    private Spell fireBlast;
    private Spell fireball;
    private Spell empower;
    private Spell[] spellbook;
    private Boolean triggersGCD;
    private Spell toCast;
    private int n;
    private Boolean canCast;

    // Use this for initialization
    void Start()
    {
        heroHealth = 100;
        enemyHealth = 100;
        heroHealthText.text = heroHealth.ToString();
        enemyHealthText.text = enemyHealth.ToString();
        cast1Text.text = "0";
        cast2Text.text = "0";
        GCDText.text = "0";
        cast3Text.text = "0";
        multiplier = 1;
        addititve = 0;
        fireBlast = new Spell(4, 1, true);
        fireball = new Spell(10, 5, true);
        empower = new Spell(20, 0, false);
        empower.SetMultiplier(2);
        empower.SetGCDRespectFalse();
        spellbook = new Spell[] { fireBlast, fireball, empower};
        triggersGCD = false;
        n = 0;
        canCast = false;
        //anim = GetComponent<Animator>();

    }



    // Update is called once per frame
    void Update() {
        ReduceCooldowns();
        CheckInput();
        Cast();
        enemyCast();
        checkDead();
    }

    //Box for testing purposes
    void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 50), (1 / Time.deltaTime).ToString());
        //GUI.Box(new Rect(0, 0, 100, 50), (Math.Ceiling(cooldown[0])).ToString());
    }

    //Checks if the input is the keybind for one of the casts, then sets values so it can be cast with Cast() function
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (spellbook[0].CanCast(GCD))
            {
                n = 0;
                canCast = true;
                hero.SetBool("Use1", true);
                enemy.SetBool("Use1", true);
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (spellbook[1].CanCast(GCD))
            {
                n = 1;
                canCast = true;
                hero.SetBool("Use2", true);
                enemy.SetBool("Use2", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (spellbook[2].CanCast(GCD))
            {
                n = 2;
                canCast = true;
                multiplier = 2;
                hero.SetBool("Use3", true);
                enemy.SetBool("Use3", true);
            }
        }    
    }


    void Cast()
    {
        if (canCast)
        {
            enemyHealth -= spellbook[n].Cast(multiplier, addititve);
            triggersGCD = spellbook[n].GetTriggersGCD();
            enemyHealthText.text = Math.Round(enemyHealth).ToString();
            if (triggersGCD)
            {
                GCD = 2;
                triggersGCD = false;
            }
            canCast = false;
            multiplier = spellbook[n].GetMultiplier();
        }
    }
    //Called once every update.  Reduces cooldowns by change in time.
    void ReduceCooldowns()
    {
        //Recudes Global Cooldown
        if (GCD > 0)
        {
            GCDText.text = (Math.Ceiling(GCD -= (Time.deltaTime))).ToString();
        }else if (GCD < 0)
        {
            GCD = 0;
        }

        //Reduces Enemy CD
        enemyCastCD -= (float)(Time.deltaTime);

        //Goes through every cooldown and reduces it
        for (int i = 0; i < spellbook.Length; i++)
        {
            spellbook[i].ReduceCooldown((float)(Time.deltaTime));

            //Updates the cooldown of the moves to the GUI
            if (i == 0)
            {
                cast1Text.text = spellbook[0].displayCD();
            }
            if (i == 1)
            {
                cast2Text.text = spellbook[1].displayCD();
            }
            if (i == 2)
            {
                cast3Text.text = spellbook[2].displayCD();
            }
        }

    }

    void enemyCast()
    {
        if(enemyCastCD <= 0)
        {
            enemyCastCD = 3;
            heroHealthText.text = (heroHealth -= 3).ToString();
        }
    }

    void checkDead()
    {
        if(heroHealth <= 0)
        {
            print("You Lose");
            enabled = false;
        }
        if(enemyHealth <= 0)
        {
            print("You Win");
            enabled = false;
         
        }
    }
}


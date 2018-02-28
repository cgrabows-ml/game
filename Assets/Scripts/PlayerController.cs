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

    private int heroHealth;
    private int enemyHealth;
    private float GCD;
    private float[] cooldown;
    private Boolean multiplier;
    private float enemyCastCD;

    // Use this for initialization
    void Start()
    {
        heroHealth = 100;
        enemyHealth = 100;
        heroHealthText.text = heroHealth.ToString();
        enemyHealthText.text = enemyHealth.ToString();
        cooldown = new float[] { 0, 0, 0, 0 };
        cast1Text.text = "0";
        cast2Text.text = "0";
        GCDText.text = "0";
        cast3Text.text = "0";
        multiplier = false;

    }

    // Update is called once per frame
    void Update() {
        ReduceCooldowns();
        CheckInput();
        enemyCast();
        checkDead();
    }

    //Box for testing purposes
    void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 50), (1 / Time.deltaTime).ToString());
        //GUI.Box(new Rect(0, 0, 100, 50), (Math.Ceiling(cooldown[0])).ToString());
    }

    //Checks if the input is the keybind for one of the casts, then attempts to cast
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cast1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cast2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Cast3();
        }
    }

    //Called once every update.  Reduces cooldowns by change in time.
    void ReduceCooldowns()
    {
        //Recudes Global Cooldown
        if (GCD > 0)
        {
            GCDText.text = (Math.Ceiling(GCD -= (Time.deltaTime))).ToString();
        }
        if (GCD < 0)
        {
            GCD = 0;
        }

        //Reduces Enemy CD
        enemyCastCD -= (float)(Time.deltaTime);

        //Goes through every cooldown and reduces it
        for (int i = 0; i < cooldown.Length; i++)
        {

            if (cooldown[i] >= 0)
            {
                cooldown[i] -= (float)(Time.deltaTime);

                //Updates the cooldown of the moves to the GUI
                if (i == 0)
                {
                    cast1Text.text = (Math.Ceiling(cooldown[i])).ToString();
                }
                if (i == 1)
                {
                    cast2Text.text = (Math.Ceiling(cooldown[i])).ToString();
                }
                if (i == 2)
                {
                    cast3Text.text = (Math.Ceiling(cooldown[i])).ToString();
                }
            }

            //Makes sure cooldowns aren't under 0
            if (cooldown[i] < 0)
            {
                cooldown[i] = 0;
            }
        }
    }


    //Casts the hard coded first ability if the cooldown is available.  Sets the cooldown if cast.
    void Cast1()
    {
        if (cooldown[0] <= 0 & GCD <= 0)
        {
            cooldown[0] = 4;
            GCD = 2;
            if (multiplier)
            {
                enemyHealthText.text = (enemyHealth -= 2).ToString();
                multiplier = false;
            }
            else
            {
                enemyHealthText.text = (enemyHealth -= 1).ToString();
            }
        }
    }

    //Casts the hard coded second ability if the cooldown is available.  Sets the cooldown if cast.
    void Cast2()
    {
        if (cooldown[1] <= 0 & GCD <= 0)
        {
            cooldown[1] = 10;
            GCD = 2;
            if (multiplier)
            {
                enemyHealthText.text = (enemyHealth -= 10).ToString();
            }
            else
            {
                enemyHealthText.text = (enemyHealth -= 5).ToString();
            }
        }
    }

    //Doubles the damage of your next Cast
    void Cast3()
    {
        if (cooldown[2] <= 0 & GCD <= 0)
        {
            cooldown[2] = 20;
            GCD = 2;
            multiplier = true;
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


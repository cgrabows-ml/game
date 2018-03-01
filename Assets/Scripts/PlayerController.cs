using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Text cast1Text;
    public Text cast2Text;
    public Text cast3Text;
    public Text heroHealthText;
    public Text enemyHealthText;
    public Text GCDText;
    public Animator hero;
    public Animator enemy;

    private float maxGCD = 2;
    private int heroHealth = 100;
    public float enemyHealth = 100;
    public float GCD = 0;
    public float multiplier = 1;
    public float additive = 0;
    private float enemyCastCD = 0;
    private List<Spell> spellbook;
    private List<SpellBinding> spellBindings = new List<SpellBinding>();

    // Use this for initialization
    void Start()
    {
        setSpellBindings();
    }

    private List<Text> getTextBoxes()
    {
        return new List<Text>() { cast1Text, cast2Text, cast3Text };
    }

    private void setSpells()
    {
        Spell fireBlast = new Spell(3, 1, "Use1");
        Spell fireball = new Spell(6, 5, "Use2");
        Spell empower = new Spell(10, 0,"Use3", false);
        empower.setGCDRespect(false);
        empower.setMultiplier(2);
        spellbook = new List<Spell>() { fireBlast, fireball, empower };
    }

    private List<KeyCode> getKeys()
    {
        return new List<KeyCode>() { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
    }

    private void setSpellBindings()
    {
        setSpells();
        List<Text> textBoxes = getTextBoxes();
        List<KeyCode> keys = getKeys();
        for (int i = 0; i < spellbook.Count; i++)
        {
            SpellBinding binding = new SpellBinding(spellbook[i], keys[i], textBoxes[i]);
            spellBindings.Add(binding);
        }
    }

    // Update is called once per frame
    void Update() {
        reduceCooldowns();
        checkInput();
        enemyCast();
        checkDead();
        updateView();
    }

    public String textConverter(float value) //TODO move to general utils class or textBox wrapper class
    {
        return Math.Ceiling(value).ToString();
    }

    public void updateView()
    {
        spellBindings.ForEach(binding => binding.updateTextBox());
        GCDText.text = textConverter(GCD);
        heroHealthText.text = textConverter(heroHealth);
        enemyHealthText.text = textConverter(enemyHealth); 
    }


    public void triggerGCD()
    {
        GCD = maxGCD;
    }

    //Checks if the input is the keybind for one of the casts, then sets values so it can be cast with Cast() function
    void checkInput()
    {
        foreach(SpellBinding binding in spellBindings)
        {
            if (Input.GetKeyDown(binding.getKey()))
            {
                if (binding.spell.canCast())
                {
                    binding.spell.cast();
                }
            }
        } 
    }

    //Called once every update.  Reduces cooldowns by change in time.
    void reduceCooldowns()
    {
        //Reduces Global Cooldown
        GCD = (float) Math.Max(GCD - Time.deltaTime, 0);

        //Reduces Enemy CD
        enemyCastCD = (float) Math.Max(enemyCastCD - Time.deltaTime, 0);

        //Goes through every cooldown and reduces it
        foreach (SpellBinding binding in spellBindings)
        {
            binding.spell.reduceCooldown();
        }

    }

    void enemyCast()
    {
        if(enemyCastCD <= 0)
        {
            enemyCastCD = 3;
            heroHealth -= 3;
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


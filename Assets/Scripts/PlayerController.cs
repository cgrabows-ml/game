using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Text cast1Text;
    public Text cast2Text;
    public Text cast3Text;
    public Text cast4Text;
    public Text heroHealthText;
    public Text enemyHealthText1;
    public Text enemyHealthText2;
    public Text enemyHealthText3;
    public Text enemyHealthText4;
    public Text GCDText;
    public Animator hero;
    public Animator enemy1;
    public Animator enemy2;
    public Animator enemy3;
    public Animator enemy4;
    public List<Enemy> enemies = new List<Enemy>();
    public float heroHealth = 100;

    private float maxGCD = 2;

    public float GCD = 0;
    public float multiplier = 1;
    public float additive = 0;
    private float enemyCastCD = 0;
    private List<Spell> spellbook;
    private List<Spell> warriorSpellbook;
    private List<Spell> mageSpellbook;
    private List<SpellBinding> spellBindings = new List<SpellBinding>();


    // Use this for initialization
    void Start()
    {
        setSpells();
        setEnemySpells();
        setSpellBindings();
        setEnemies();
    }

    /// <summary>
    /// Initializes spells to be used for enemies.  Puts spells into spellbooks for the enemies.
    /// </summary>
    private void setEnemySpells()
    {
        Spell stab = new Spell(4, 1, "Use1", target: "player");
        Spell slash = new Spell(6, 5, "Use2", target: "player");
        Spell splash = new Spell(4, 1, "Use1", target: "player");
        Spell frostbolt = new Spell(6, 5, "Use2", target: "player");
        warriorSpellbook = new List<Spell> { stab, slash };
        mageSpellbook = new List<Spell> { splash, frostbolt };
    }

    /// <summary>
    /// Initializes Enemies.
    /// </summary>
    private void setEnemies()
    {
        Enemy warrior = new Enemy("warrior", 3, warriorSpellbook, enemyHealthText1, enemy1, 120);
        Enemy mage = new Enemy("mage", 2, mageSpellbook, enemyHealthText2, enemy2, 80);
        enemies = new List<Enemy> { warrior, mage };
    }

    /// <summary>
    /// Gets a list of text boxes.
    /// </summary>
    /// <returns>The list of text boxes</returns>
    private List<Text> getTextBoxes()
    {
        return new List<Text>() {cast1Text, cast2Text, cast3Text, cast4Text };
    }

    /// <summary>
    /// Initializes spells.
    /// </summary>
    private void setSpells()
    {
        Spell fireBlast = new Spell(3, 3, "Use1");
        Spell fireball = new Spell(6, 5, "Use2", target:"back");
        Spell splash = new Spell(8, 1, "Use3", target: "AoE");
        Spell empower = new Spell(10, 0,"Use4", false);
        empower.setGCDRespect(false);
        empower.setMultiplier(2);
        spellbook = new List<Spell>() { fireBlast, fireball, splash, empower };
    }

    /// <summary>
    /// Returns a list of keybinds to be used for spells.
    /// </summary>
    /// <returns>A list of keybinds.</returns>
    private List<KeyCode> getKeys()
    {
        return new List<KeyCode>() { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    }

    /// <summary>
    /// Initializes spellBindings to tie together the keybinds, spells, and text boxes.
    /// </summary>
    private void setSpellBindings()
    {
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

    /// <summary>
    /// A function that gets the ceiling of a value, then converts it to a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The string of the ceiling of the input.</returns>
    public String textConverter(float value) //TODO move to general utils class or textBox wrapper class
    {
        return Math.Ceiling(value).ToString();
    }

    /// <summary>
    /// Updates all the text on the screen.
    /// </summary>
    public void updateView()
    {
        spellBindings.ForEach(binding => binding.updateTextBox());
        enemies.ForEach(enemy => enemy.UpdateTextBox());
        GCDText.text = textConverter(GCD);
        heroHealthText.text = textConverter(heroHealth);
    }


    public void triggerGCD()
    {
        GCD = maxGCD;
    }

    /// <summary>
    /// Checks if the input is the keybind for one of the casts, then casts the spell if it's valid to cast.
    /// </summary>
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

    /// <summary>
    /// Reduces GCD, enemy CDs, and hero's spell CDs.
    /// </summary>
    void reduceCooldowns()
    {
        //Reduces Global Cooldown
        GCD = (float) Math.Max(GCD - Time.deltaTime, 0);

        //Reduces Enemy CD
        foreach(Enemy enemy in enemies)
        {
            enemy.ReduceCooldowns();
        }

        //Goes through every cooldown and reduces it
        foreach (SpellBinding binding in spellBindings)
        {
            binding.spell.reduceCooldown();
        }

    }

    /// <summary>
    /// Called from Update.  Goes through all enemy spells and casts whichever is available, by which is first in the spellbook.
    /// </summary>
    void enemyCast()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Cast();
        }
    }

    void checkDead()
    {
        //ToDo: the whole function lmao
    }
}


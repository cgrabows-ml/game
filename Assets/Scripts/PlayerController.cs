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
    public Animator heroAnim;
    public Animator enemy1Anim;
    public Animator enemy2Anim;
    public Animator enemy3Anim;
    public Animator enemy4Anim;
    public List<Enemy> enemies = new List<Enemy>();
    public List<Spell> warriorSpellbook;
    public List<Spell> mageSpellbook;
    public List<Spell> spellbook;
    public Character hero;

    private List<SpellBinding> spellBindings = new List<SpellBinding>();


    // Use this for initialization
    void Start()
    {
        SetSpells();
        SetHero();
        SetEnemySpells();
        SetSpellBindings();
        SetEnemies();
    }

    private void SetHero()
    {
        hero = new Hero(spellbook, heroHealthText, heroAnim, 100);
    }

    /// <summary>
    /// Initializes Hero spells.
    /// </summary>
    private void SetSpells()
    {
        Spell fireBlast = new DamageSpell(3, 1, "Use1");
        Spell fireball = new DamageSpell(6, 2, "Use2", target: "back");
        Spell splash = new DamageSpell(8, 3, "Use3", target: "AoE");
        Spell empower = new Empower();
        spellbook = new List<Spell>() { fireBlast, fireball, splash, empower };
    }


    /// <summary>
    /// Initializes spells to be used for enemies.  Puts spells into spellbooks for the enemies.
    /// </summary>
    private void SetEnemySpells()
    {
        Spell stab = new DamageSpell(4, 1, "Use1", target: "player");
        Spell slash = new DamageSpell(6, 5, "Use2", target: "player");
        Spell splash = new DamageSpell(4, 1, "Use1", target: "player");
        Spell frostbolt = new DamageSpell(6, 5, "Use2", target: "player");
        warriorSpellbook = new List<Spell> { stab, slash };
        mageSpellbook = new List<Spell> { splash, frostbolt };
    }

    /// <summary>
    /// Initializes Enemies.
    /// </summary>
    private void SetEnemies()
    {
        Enemy warrior = new Enemy("warrior", 3, warriorSpellbook, enemyHealthText1, enemy1Anim, 120);
        Enemy mage = new Enemy("mage", 2, mageSpellbook, enemyHealthText2, enemy2Anim, 80);
        enemies = new List<Enemy> { warrior, mage };
    }

    /// <summary>
    /// Gets a list of text boxes.
    /// </summary>
    /// <returns>The list of text boxes</returns>
    private List<Text> GetTextBoxes()
    {
        return new List<Text>() {cast1Text, cast2Text, cast3Text, cast4Text };
    }

    /// <summary>
    /// Returns a list of keybinds to be used for spells.
    /// </summary>
    /// <returns>A list of keybinds.</returns>
    private List<KeyCode> GetKeys()
    {
        return new List<KeyCode>() { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    }

    /// <summary>
    /// Initializes spellBindings to tie together the keybinds, spells, and text boxes.
    /// </summary>
    private void SetSpellBindings()
    {
        List<Text> textBoxes = GetTextBoxes();
        List<KeyCode> keys = GetKeys();
        for (int i = 0; i < spellbook.Count; i++)
        {
            SpellBinding binding = new SpellBinding(hero.spellbook[i], keys[i], textBoxes[i]);
            spellBindings.Add(binding);
        }
    }

    // Update is called once per frame
    void Update() {
        CheckInput();
        enemies.ForEach(enemy => enemy.Update());
        hero.Update();
        CheckDead();
        UpdateView();
    }

    /// <summary>
    /// Updates all the text on the screen.
    /// </summary>
    public void UpdateView()
    {
        spellBindings.ForEach(binding => binding.Update());
        GCDText.text = Utils.ToDisplayText(hero.GCD);
    }

    /// <summary>
    /// Checks if the input is the keybind for one of the casts, then casts the spell if it's valid to cast.
    /// </summary>
    void CheckInput()
    {
        int i = 0;
        foreach(SpellBinding binding in spellBindings)
        {
            if (Input.GetKeyDown(binding.GetKey()))
            {
                //CanCast also does the cast
                hero.CanCast(i);
            }
            i++;
        } 
    }
    
    /*
    /// <summary>
    /// Called from Update.  Goes through all enemy spells and casts whichever is available, by which is first in the spellbook.
    /// </summary>
    void EnemyCast()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Cast();
        }
    }*/

    void CheckDead()
    {
        //ToDo: the whole function lmao
    }
}


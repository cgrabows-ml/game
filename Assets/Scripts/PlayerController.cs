using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;
    public Text cast1Text;
    public Text cast2Text;
    public Text cast3Text;
    public Text cast4Text;
    public Text GCDText;
    public List<Enemy> enemies = new List<Enemy>();
    public List<Spell> mageSpellbook;
    public List<Spell> spellbook = new List<Spell>() { };
    public Character hero;
    public static string TextField;
    public RectTransform castCover1;
    public RectTransform castCover2;
    public RectTransform castCover3;
    public RectTransform castCover4;

    public TextMesh heroHealthText;

    private List<SpellBinding> spellBindings = new List<SpellBinding>();
    private Transform instance;
    private List<Transform> enemyGUI = new List<Transform> { };
    private int lastX = 0;



    // Use this for initialization
    void Start()
    {
        SetHero();
        SetSpells();
        SetSpellToolTips();
        SetEnemySpells();
        SetSpellBindings();
        SetEnemies();
        playerController = this;
    }


    private void SetSpellToolTips()
    {
        List<TooltipHover> tooltips = new List<TooltipHover>(FindObjectsOfType<TooltipHover>());

        //Sort by transform x position
        int j = 1;
        while(j < tooltips.Count)
        {
            if (tooltips[j].transform.position.x <= tooltips[j - 1].transform.position.x)
            {
                TooltipHover t = tooltips[j - 1];
                tooltips[j - 1] = tooltips[j];
                tooltips[j] = t;
                j = 1;
            }
            else
            {
                j++;
            }
        }


        int i = 0;
        foreach (TooltipHover tooltip in tooltips)
        {
            tooltip.spell = hero.spellbook[i];
            i++;
        }
    }

    private void SetHero()
    {
        hero = new Hero(heroHealthText);

    }

    /// <summary>
    /// Initializes Hero spells.
    /// </summary>
    private void SetSpells()
    {
        foreach(Spell spell in hero.spellbook)
        {
            spellbook.Add(spell);
        }
    }


    /// <summary>
    /// Initializes spells to be used for enemies.  Puts spells into spellbooks for the enemies.
    /// </summary>
    private void SetEnemySpells()
    {
        Spell splash = new DamageSpell(4, 1, "Use1", target: "player");
        Spell frostbolt = new DamageSpell(6, 5, "Use2", target: "player");
        mageSpellbook = new List<Spell> { splash, frostbolt };
    }

    /// <summary>
    /// Initializes Enemies.
    /// </summary>
    private void SetEnemies()
    {
        Enemy warrior = new Warrior(new Vector3(-0.3f + 1.43f * 0, -2.58f, 0));
        Enemy mage = new Enemy("mage", 2, mageSpellbook, (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/warrior.prefab", typeof(Transform)), (TextMesh)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/enemy_text.prefab", typeof(TextMesh)),
            new Vector3(-0.3f + 1.43f * 2, -2.58f, 0), 2);
        Enemy warrior2 = new Warrior(new Vector3(-0.3f + 1.43f * 1, -2.58f, 0));
        enemies = new List<Enemy> { warrior, warrior2, mage };
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
        UpdateView();
    }

    /// <summary>
    /// Updates all the text on the screen.
    /// </summary>
    public void UpdateView()
    {
        spellBindings.ForEach(binding => binding.Update());
        //foreach update characterbinding
        GCDText.text = Utils.ToDisplayText(hero.GCD);
    }

    /// <summary>
    /// Checks if the input is the keybind for one of the casts, then casts the spell if it's valid to cast.
    /// </summary>
    void CheckInput()
    {
        foreach(SpellBinding binding in spellBindings)
        {
            if (Input.GetKeyDown(binding.GetKey()))
            {
                //CanCast also does the cast
                hero.CastIfAble(binding.spell);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddEnemy(new Warrior(new Vector3(-0.3f + 1.43f * 2, -2.58f, 0)));
        }
    }

    //Removes enemy from enemy List
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    //Adds enemy to enemy list and instantiates
    public void AddEnemy(Enemy enemy)
    {
        List<Enemy> newEnemy = new List<Enemy> { };
        enemies.Add(enemy);
        newEnemy.Add(enemy);
    }

}


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
    public static string TextField;

    public Transform herofab;
    public Transform warriorfab;
    public Transform healthbarFab;
    public Transform healthTextFab;
    //public Transform mage;

    public TextMesh enemyHealthText1;
    public TextMesh enemyHealthText2;
    public TextMesh enemyHealthText3;
    public TextMesh enemyHealthText4;
    public TextMesh heroHealthText;

    private List<SpellBinding> spellBindings = new List<SpellBinding>();
    private List<CharacterBinding> enemyBindings = new List<CharacterBinding>();
    private Transform instance;
    private List<Transform> enemyGUI = new List<Transform> { };


    // Use this for initialization
    void Start()
    {
        SetSpells();
        SetHero();
        SetEnemySpells();
        SetSpellBindings();
        SetEnemies();
        SetEnemyBindings();
        EnemyInstantiate();

        //This is temp. Remove this pls.
        //Instantiate(warrior, new Vector3(0, 0, 0), Quaternion.identity);
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
        Enemy warrior = new Warrior(enemyHealthText1, enemy1Anim);
        Enemy mage = new Enemy("mage", 2, mageSpellbook, enemyHealthText1,  enemy2Anim, 2);
        Enemy warrior2 = new Warrior(enemyHealthText1, enemy3Anim);
        enemies = new List<Enemy> { warrior, warrior2, mage };

    }

    private void SetEnemyBindings()
    {
        List<TextMesh> textBoxes = new List<TextMesh>{ enemyHealthText1, enemyHealthText2, enemyHealthText3 };
        List<Transform> prefabs = new List<Transform> { warriorfab, warriorfab, warriorfab };
        for (int i = 0; i < enemies.Count; i++)
        {
            CharacterBinding binding = new CharacterBinding(enemies[i], textBoxes[i], prefabs[i].GetComponent<Animator>(), prefabs[i]);
            enemyBindings.Add(binding);
        }
    }


    private void EnemyInstantiate()
    {
        int i = 0;
        foreach (CharacterBinding binding in enemyBindings)
        {
            //Starting enemy spawn point
            Vector3 vector = new Vector3(-0.3f + 1.43f * i, -2.58f, 0);

            //Instantiate Enemy 
            instance = Instantiate(binding.prefab, vector, Quaternion.identity);
            binding.character.anim = instance.GetComponent<Animator>();
            enemyGUI.Add(instance);

            //Instantiate Enemy Health Bar
            instance = Instantiate(healthbarFab, vector + new Vector3(0f, 1.8f, 0f), Quaternion.identity);
            enemyGUI.Add(instance);


            //Instantiate Text
            instance = Instantiate(healthTextFab, vector + new Vector3(0f, 1.8f, 0f), Quaternion.identity);
            binding.character.textBox = instance.GetComponent<TextMesh>();
            enemyGUI.Add(instance);
            i++;

            binding.character.instances = enemyGUI;
            enemyGUI = new List<Transform>{ };
        }
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
        //foreach update characterbinding
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            //SpawnEnemy();
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
        //Checks if an enemy has died.  Removes them from the enemies list and the enemyBindings list
        List<Character> toRemove = new List<Character> { };
        List<CharacterBinding> toRemoveB = new List<CharacterBinding> { };
        int i = 0;
        foreach(CharacterBinding binding in enemyBindings)
        {
            //todo move the check for being dead in the Character class, this is temp
            if(binding.character.health <= 0)
            {
                toRemove.Add(binding.character);
                toRemoveB.Add(binding);
            }
            i++;
        }
        foreach(Enemy chara in toRemove)
        {
            //Destroies in GUI
            foreach(Transform instance in chara.instances)
            {
                Destroy(instance.gameObject);
            }
            enemies.Remove(chara);
        }
        foreach(CharacterBinding binding in toRemoveB)
        {
            enemyBindings.Remove(binding);
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public GameController gc;
    public Text cast1Text;
    public Text cast2Text;
    public Text cast3Text;
    public Text cast4Text;
    public Text GCDText;

    public List<Spell> spellbook = new List<Spell>() { };
    public Hero hero;
    public Canvas canvas;

    public Transform herofab;
    public Transform warriorfab;
    public Transform healthbarFab;
    public Transform healthTextFab;

    public Stage stage;
    
    public List<Enemy> enemies = new List<Enemy>();
    public List<Spell> mageSpellbook;
    public static string TextField;
    public RectTransform castCover1;
    public RectTransform castCover2;
    public RectTransform castCover3;
    public RectTransform castCover4;
    public Text cantCast;

    public Text spaceContinue;

    public Camera cam;

    public TextMesh heroHealthText;
    public TextMesh heroEnergyText;

    public RectTransform tinyBox1;
    public RectTransform tinyBox2;
    public RectTransform tinyBox3;
    public RectTransform tinyBox4;

    public RectTransform bigBox1;
    public RectTransform bigBox2;
    public RectTransform bigBox3;
    public RectTransform bigBox4;

    public Timer timer = new Timer();

    public List<SpellBinding> spellBindings = new List<SpellBinding>();
    private Transform instance;
    private OrderedDictionary bufferedSpellBinding = new OrderedDictionary();
    private Dictionary<KeyCode, int> bufferedSpellBindingCounts = new Dictionary<KeyCode,int>();
    // Use this for initialization
    void Start()
    {
        gameController = GetComponent<GameController>();
        SetStage();
        SetHero();
        SetSpells();
        SetSpellBindings();
        SetSpellToolTips();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    private void SetStage()
    {
        stage = new Stage();
        //Encounter encounter = new Encounter(stage, enemies);
        List<Encounter> encounters = new List<Encounter>() {
            new NecromancerBossEncounter(stage) };
        stage.SetEncounters(encounters);
        stage.StartStage();
    }

    private void SetHero()
    {
        this.hero = stage.hero;
    }


    private void SetSpellToolTips()
    {
        //SLOW dont use find
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


        tinyBox1.GetComponent<KeypressDownTiny>().k = spellBindings[0].GetKey();
        tinyBox2.GetComponent<KeypressDownTiny>().k = spellBindings[1].GetKey();
        tinyBox3.GetComponent<KeypressDownTiny>().k = spellBindings[2].GetKey();
        tinyBox4.GetComponent<KeypressDownTiny>().k = spellBindings[3].GetKey();

        bigBox1.GetComponent<KeypressDownBig>().k = spellBindings[0].GetKey();
        bigBox2.GetComponent<KeypressDownBig>().k = spellBindings[1].GetKey();
        bigBox3.GetComponent<KeypressDownBig>().k = spellBindings[2].GetKey();
        bigBox4.GetComponent<KeypressDownBig>().k = spellBindings[3].GetKey();

        castCover1.GetComponent<KeypressShrink>().k = spellBindings[0].GetKey();
        castCover2.GetComponent<KeypressShrink>().k = spellBindings[1].GetKey();
        castCover3.GetComponent<KeypressShrink>().k = spellBindings[2].GetKey();
        castCover4.GetComponent<KeypressShrink>().k = spellBindings[3].GetKey();

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

    private List<RectTransform> GetCastCovers()
    {
        return new List<RectTransform> { gameController.castCover1,
            gameController.castCover2,
            gameController.castCover3,
            gameController.castCover4 };
    }

    /// <summary>
    /// Initializes spellBindings to tie together the keybinds, spells, and text boxes.
    /// </summary>
    private void SetSpellBindings()
    {
        List<RectTransform> castCovers = GetCastCovers();
        List<Text> textBoxes = GetTextBoxes();
        List<KeyCode> keys = GetKeys();
        for (int i = 0; i < spellbook.Count; i++)
        {
            SpellBinding binding = new SpellBinding(hero.spellbook[i], keys[i], textBoxes[i],
                castCovers[i]);
            spellBindings.Add(binding);
        }
    }

    // Update is called once per frame
    void Update() {
        stage.Update();
        if (stage.inCombat)
        {
            timer.Increment();
            CheckInput();
            hero.Update();
            UpdateView();
        }

    }

    /// <summary>
    /// ; all the text on the screen.
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
        var keysToRemove = new List<KeyCode>();
        foreach (SpellBinding binding in spellBindings)
        {
            KeyCode key = binding.GetKey();
            if (Input.GetKeyDown(key))
            {
                // a usable key has been pressed
                // add this key to a 'list' of keys that were pressed recently
                // order matters

                // update ordering if that key has already been pressed recently
                if (bufferedSpellBinding.Contains(key))
                {
                    bufferedSpellBindingCounts[key] --;
                    bufferedSpellBinding.Remove(key);
                }
                else{
                    bufferedSpellBindingCounts[key] = 0;
                }
                bufferedSpellBinding.Add(key, binding);
                bufferedSpellBindingCounts[key] ++;
                // after 30 frames, no longer recently pressed
                timer.SetTimer(30, () => {
                    if (bufferedSpellBindingCounts.ContainsKey(key)){
                    
                        bufferedSpellBindingCounts[key] --;
                        if (bufferedSpellBindingCounts[key] == 0){
                            bufferedSpellBinding.Remove(key);
                            hero.CantCastMessage();
                        }
                    }
                    
                });
            }
        }   
        // loop over recently pressed keys, and attempt to apply them
        foreach (KeyCode key in bufferedSpellBinding.Keys)
        {
            SpellBinding binding = (SpellBinding)(bufferedSpellBinding[key]);
            if (binding.spell.isCastable()){
                hero.Cast(binding.spell);
                // bufferedSpellBinding.Remove(key);
                keysToRemove.Add(key);
            }
        }
        foreach (KeyCode key in keysToRemove){
            bufferedSpellBinding.Remove(key);
            bufferedSpellBindingCounts.Remove(key);
        }
    }
}


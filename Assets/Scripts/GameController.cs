using System;
using System.Collections;
using System.Collections.Generic;
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

    public Transform heroHealthText;
    public Transform energy1;
    public Transform energy2;
    public Transform energy3;
    public Transform energy4;
    public Transform energy5;

    public List<Transform> energyUI;

    public RectTransform tinyBox1;
    public RectTransform tinyBox2;
    public RectTransform tinyBox3;
    public RectTransform tinyBox4;

    public RectTransform bigBox1;
    public RectTransform bigBox2;
    public RectTransform bigBox3;
    public RectTransform bigBox4;

    public Boolean isPaused = false;

    public List<SpellUI> spellUIs = new List<SpellUI>();

    // Use this for initialization
    void Start()
    {
        gameController = GetComponent<GameController>();
        SetEnergyUI();
        SetStage();
        SetHero();
        SetSpellUIs();
        //SetSpellToolTips();

        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    private void SetEnergyUI()
    {
        energyUI = new List<Transform>() { energy1, energy2, energy3, energy4, energy5 };
    }

    private void SetStage()
    {
        stage = new TutorialStage();
        //stage = new NecromancerBossStage();
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
        
        tinyBox1.GetComponent<KeypressDownTiny>().k = spellUIs[0].GetKey();
        tinyBox2.GetComponent<KeypressDownTiny>().k = spellUIs[1].GetKey();
        tinyBox3.GetComponent<KeypressDownTiny>().k = spellUIs[2].GetKey();
        tinyBox4.GetComponent<KeypressDownTiny>().k = spellUIs[3].GetKey();

        bigBox1.GetComponent<KeypressDownBig>().k = spellUIs[0].GetKey();
        bigBox2.GetComponent<KeypressDownBig>().k = spellUIs[1].GetKey();
        bigBox3.GetComponent<KeypressDownBig>().k = spellUIs[2].GetKey();
        bigBox4.GetComponent<KeypressDownBig>().k = spellUIs[3].GetKey();

        castCover1.GetComponent<KeypressShrink>().k = spellUIs[0].GetKey();
        castCover2.GetComponent<KeypressShrink>().k = spellUIs[1].GetKey();
        castCover3.GetComponent<KeypressShrink>().k = spellUIs[2].GetKey();
        castCover4.GetComponent<KeypressShrink>().k = spellUIs[3].GetKey();

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
    /// Initializes spellUIs to tie together the keybinds, spells, and text boxes.
    /// </summary>
    private void SetSpellUIs()
    {
        List<RectTransform> castCovers = GetCastCovers();
        List<Text> textBoxes = GetTextBoxes();
        List<KeyCode> keys = GetKeys();
        for (int i = 0; i < hero.spellbook.Count; i++)
        {
            SpellUI binding = new SpellUI(hero.spellbook[i], keys[i], i);
            spellUIs.Add(binding);
        }
    }

    // Update is called once per frame
    void Update() {
        stage.Update();
        hero.Update();
        CheckInput();


        if (stage.inCombat)
        {
            UpdateView();
        }

    }

    /// <summary>
    /// ; all the text on the screen.
    /// </summary>
    public void UpdateView()
    {
        spellUIs.ForEach(binding => binding.Update());
        //foreach update characterbinding
        GCDText.text = Utils.ToDisplayText(hero.GCD);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Checks if the input is the keybind for one of the casts, then casts the spell if it's valid to cast.
    /// </summary>
    void CheckInput()
    {
        if (Input.GetKeyDown("p"))
        {
            TogglePause();
        }
        if (!isPaused && stage.inCombat)
        {
            foreach (SpellUI binding in spellUIs)
            {
                if (Input.GetKeyDown(binding.GetKey()))
                {
                    //CanCast also does the cast
                    hero.CastIfAble(binding.spell);
                }
            }
        }
    }
}


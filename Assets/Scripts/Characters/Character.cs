using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public abstract class Character
{
    public static GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

    public List<Spell> spellbook;
    private float maxHealth;
    public float health;
    public float GCD = 0;
    public float maxGCD;
    public float inAdditive = 0;
    public float outAdditive = 0;
    public float inMultiplier = 1;
    public float outMultiplier = 1;
    public float deathTime = 2;
    public TextMesh textBox;
    public Animator anim;
    public List<Buff> buffs = new List<Buff> { };
    public List<Transform> instances = new List<Transform> { };
    public List<TextMesh> floatingCombatText = new List<TextMesh> { };
    public Transform prefab;
    protected List<SpellCastObserver> spellCastObservers = new List<SpellCastObserver>();

    /// <summary>
    /// Constructor for Character class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="max_health"></param>
    public Character(String prefabPath, TextMesh textBox,
        float maxHealth = 10,
        float maxGCD = 2)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.spellbook = getSpells();
        this.textBox = textBox;
        this.maxGCD = maxGCD;
        this.prefab = getPrefab(prefabPath);
        //this.anim = prefab.GetComponent<Animator>();        
    }

    private static Transform getPrefab(String path)
    {
        string prefabPath = "Assets/Prefabs/" + path; //TODO: read from config or other
        return (Transform)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Transform));
    }

    protected abstract List<Spell> getSpells();

    public virtual void Spawn(Vector2 pos)
    {
        Transform instance = MonoBehaviour.Instantiate(prefab);
        instance.position = pos;
        instances.Add(instance);
        anim = instance.GetComponent<Animator>();
        textBox.text = Utils.ToDisplayText(health);


    }

    //Also casts the spell
    public virtual Boolean CastIfAble(Spell spell)
    {
        if (!spellbook.Contains(spell))
        {
            //Throw error
            MonoBehaviour.print("Spellbook does not contain spell.");
        }
        Boolean castable = spell.isCastable();
        if (castable)
        {
            anim.SetBool(spell.animationKey, true);
            spell.Cast();
            spellCastObservers.ForEach(observer => observer.SpellCastUpdate(spell, this));
        }

        return castable;
    }

    /// <summary>
    /// Register object that will listen for spellcasts.
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterCastListener(SpellCastObserver observer)
    {
        spellCastObservers.Add(observer);
    }

    /// <summary>
    /// Unregister object that will listen for spellcasts.
    /// </summary>
    /// <param name="observer"></param>
    public void UnregisterCastListener(SpellCastObserver observer)
    {
        spellCastObservers.Remove(observer);
    }

    //Right now this removes buffs every update... should be changed
    public void Update()
    {
        ReduceCooldowns();
        buffs.ForEach(buff => buff.Update());
    }

    /// <summary>
    /// Reduces GCD and cooldowns of every spell in the spellbook
    /// </summary>
    public void ReduceCooldowns()
    {
        //Reduces GCD
        GCD = Math.Max(GCD - Time.deltaTime, 0);

        //Reduces Spell CDs
        spellbook.ForEach(spell => spell.ReduceCooldown());
    }

    /// <summary>
    /// Sets the max GCD.
    /// </summary>
    /// <param name="newMaxGCD"></param>
    public void SetMaxGCD(float maxGCD)
    {
        this.maxGCD = maxGCD;
    }

    /// <summary>
    /// Get damage the character should deal given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public float GetDamage(float baseDamage)
    {
        return outAdditive + (outMultiplier * baseDamage);
    }

    /// <summary>
    /// Takes damage given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    public virtual float TakeDamage(float baseDamage, Character source)
    {
        float damageTaken = inAdditive + (inMultiplier * baseDamage);
        health -= damageTaken;
        health = Math.Min(maxHealth, health);
        health = Math.Max(health, 0);
        textBox.text = Utils.ToDisplayText(health);
        DrawDamageTaken(damageTaken);
        CheckDeadAndKill();
        return damageTaken;
    }

    public virtual void CheckDeadAndKill()
    { 
        if (health <= 0) {
            foreach (Transform instance in instances)
            {
                MonoBehaviour.print("You Lose.");
                //MonoBehaviour.Destroy(instance.gameObject);
            }
        }
    }

    /// <summary>
    /// Draws damage taken on the screen next to the character that took damage with a minus sign.
    /// </summary>
    /// <param name="damageTaken"></param>
    public void DrawDamageTaken(float damageTaken) { 
    
        Transform prefab = (Transform)AssetDatabase
            .LoadAssetAtPath("Assets/Prefabs/FCT.prefab", typeof(Transform));
        Transform instance = MonoBehaviour.Instantiate(prefab);
        TextMesh tmesh = instance.GetComponent<TextMesh>();
        String symbol = "-";
        if (damageTaken < 0)
        {
            symbol = "+";
        }
        tmesh.text = symbol + " " + Utils.ToDisplayText(Math.Abs(damageTaken));

        //Assigns color to text based on amount of mitigation        
        if(inMultiplier > 1)
        {
            tmesh.color = new Color(255, 255, 0, 1f);
        }
        else if(inMultiplier < 1){
            tmesh.color = Color.blue;
        }

        if (damageTaken < 0)
        {
            tmesh.color = Color.green;
        }

        //Assigns size based on sqrt of damage
        tmesh.fontSize = (int)Math.Round(tmesh.fontSize * Math.Sqrt(Math.Abs(damageTaken)));

        Vector3 newPos = new Vector3(instances[0].position.x + .2f,
            instances[0].position.y + .4f, 0);
        instance.transform.position = newPos;
        
        IEnumerator coroutine = DestroyFCT(instance, 1.5f);
        gameController.StartCoroutine(coroutine);
    }

    /// <summary>
    /// Handles Floating Combat Text until it's destroied
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IEnumerator DestroyFCT(Transform instance, float time)
    {
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            instance.transform.position += new Vector3(.31f * Time.deltaTime, 0.62f * Time.deltaTime, 0);
            yield return null;
        }
        MonoBehaviour.Destroy(instance.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public void TriggerGCD()
    {
        GCD = maxGCD;
    }

    public IEnumerator DestroyAfterTime(float time)
    {
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        foreach (Transform instance in instances)
        {
            MonoBehaviour.Destroy(instance.gameObject);
        }
    }
}




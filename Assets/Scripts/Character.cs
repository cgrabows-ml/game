using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class Character : GameLogger
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public List<Spell> spellbook;
    private float maxHealth;
    public float health;
    public float GCD = 0;
    public float maxGCD = 2;
    public float inAdditive = 0;
    public float outAdditive = 0;
    public float inMultiplier = 1;
    public float outMultiplier = 1;
    public TextMesh textBox;
    public Animator anim;
    public List<Buff> buffs = new List<Buff> { };
    public List<Transform> instances = new List<Transform> { };
    public List<TextMesh> floatingCombatText = new List<TextMesh> { };
    public Transform prefab;
    private List<SpellCastObserver> spellCastObservers = new List<SpellCastObserver>();

    /// <summary>
    /// Constructor for Character class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="max_health"></param>
    public Character(List<Spell> spellbook, Transform prefab, TextMesh textBox, float maxHealth = 100)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.spellbook = spellbook;
        this.prefab = prefab;
        this.textBox = textBox;
        this.anim = prefab.GetComponent<Animator>();        
    }

    //Also casts the spell
    public Boolean CastIfAble(Spell spell)
    {
        if (!spellbook.Contains(spell))
        {
            //Throw error
            error("Spellbook does not contain spell.");
        }
        Boolean castable = (spell.GetCooldown() <= 0 && (GCD <= 0 || spell.GCDRespect == false));
        if (castable)
        {
            spell.Cast(this);
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
    public void TakeDamage(float baseDamage)
    {
        float damageTaken = inAdditive + (inMultiplier * baseDamage);
        health -= damageTaken;
        textBox.text = Utils.ToDisplayText(Math.Max(health, 0));
        DrawDamageTaken(damageTaken);
        CheckDeadAndKill();
    }

    public virtual void CheckDeadAndKill()
    { 
        if (health <= 0) {
            foreach (Transform instance in instances)
            {
                MonoBehaviour.Destroy(instance.gameObject);
            }
        }
    }

    /// <summary>
    /// Draws damage taken on the screen next to the character that took damage with a minus sign.
    /// </summary>
    /// <param name="damageTaken"></param>
    public void DrawDamageTaken(float damageTaken) { 
    
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/enemy_text.prefab", typeof(Transform));
        Transform instance = MonoBehaviour.Instantiate(prefab);
        TextMesh tmesh = instance.GetComponent<TextMesh>();
        tmesh.text = "- " + Utils.ToDisplayText(damageTaken);
        Vector3 newPos = new Vector3(instances[0].position.x + .2f, instances[0].position.y + .4f, 0);
        instance.transform.position = newPos;
        
        IEnumerator coroutine = DestroyFCT(instance, 1.5f);
        playerController.StartCoroutine(coroutine);
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
            instance.transform.position += new Vector3(.005f, 0.01f, 0);
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

}




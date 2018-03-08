using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class Enemy : Character
{
    private string name;
    private List<Transform> enemyGUI = new List<Transform> { };
    private List<DeathObserver> deathObservers = new List<DeathObserver>();

    /// <summary>
    /// Constructor for enemy class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="castFreq"></param>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    public Enemy(string name, float castFreq, List<Spell> spellbook, Transform prefab, TextMesh textBox, Vector3 position, float health = 100)
        : base(spellbook, prefab, textBox, health)
    {
        maxGCD = castFreq;
        InstantiateEnemy(position);
    }

    new public void Update()
    {
        base.Update();
        Cast();
    }

    /// <summary>
    /// This is the AI for the enemy.  The enemy goes through his spellbook and sees if it's valid for him to cast each spell.  If so, he casts it.  This also triggers the enemy GCD, and animates the enemy.
    /// </summary>
    public void Cast()
    {
        spellbook.ForEach(spell => CastIfAble(spell));
    }

    public override void CheckDeadAndKill()
    {
        if (health <= 0)
        {
            anim.SetBool("Death", true);
            playerController.stage.RemoveEnemy(this, instances);
            deathObservers.ForEach(observer => observer.DeathUpdate(this))
            IEnumerator coroutine = DestroyAfterTime(2, instances);
            playerController.StartCoroutine(coroutine);
        }

    }


    public void InstantiateEnemy(Vector3 position)
    {
        Transform instance;
        Transform healthTextFab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/enemy_text.prefab", typeof(Transform));

        //Instantiate Enemy 
        instance = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity);
        anim = instance.GetComponent<Animator>();
        enemyGUI.Add(instance);

        //Instantiate Enemy Health Bar
        instance = MonoBehaviour.Instantiate((Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/healthbar_sprite.prefab", typeof(Transform)), position + new Vector3(0f, 1.8f, 0f), Quaternion.identity);

        enemyGUI.Add(instance);


        //Instantiate Text
        instance = MonoBehaviour.Instantiate(healthTextFab, position + new Vector3(0f, 1.8f, 0f), Quaternion.identity);
        textBox = instance.GetComponent<TextMesh>();
        textBox.text = Utils.ToDisplayText(health);
        enemyGUI.Add(instance);

        instances = enemyGUI;
        enemyGUI = new List<Transform> { };
    }

    public void RegisterDeathObserver(DeathObserver observer)
    {
        deathObservers.Add(observer);
    }

    public void UnregisterDeathObserver(DeathObserver observer)
    {
        deathObservers.Remove(observer);
    }

}



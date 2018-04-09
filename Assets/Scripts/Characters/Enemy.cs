using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public abstract class Enemy : Character
{
    public string name;
    private List<Transform> enemyGUI = new List<Transform> { };
    private List<IDeathObserver> deathObservers = new List<IDeathObserver>();

    public float width;
    public Boolean isFixed = false;
    public Vector2 moveTo = new Vector3(-100,0);
    private float walkSpeed = 1;
    private Boolean isMoving = false;
    public Boolean isActive = false;

    /// <summary>
    /// Constructor for enemy class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    public Enemy(string name,String prefabPath, TextMesh textBox,
        float health = 100, float width = 1,
        float maxGCD = 2)
        : base(prefabPath, textBox, health)
    {
        this.width = width;
        this.name = name;
    }

    new public void Update()
    {
        base.Update();
        Cast();
    }

    /// <summary>
    /// This is the AI for the enemy.The enemy goes through his spellbook
    /// and sees if it's valid for him to cast.
    /// This also triggers the enemy GCD, and animates the enemy.
    /// </summary>
    public void Cast()
    {
        if (isActive)
        {
            spellbook.ForEach(spell => CastIfAble(spell));
        }
    }

    public override void CheckDeadAndKill()
    {
        if (health <= 0)
        {
            //Add to total kill number
            VictoryStats.enemiesSlain += 1;

            //Move enemy and uninstantiate
            anim.SetBool("Death", true);
            moveTo = instances[0].position;
            isActive = false;
            deathObservers.ForEach(observer => observer.DeathUpdate(this));
            IEnumerator coroutine = DestroyAfterTime(deathTime);
            gameController.StartCoroutine(coroutine);
        }

    }

    public Boolean CheckDead()
    {
        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Spawn(Vector2 pos)
    {
        isActive = true;
        InstantiateEnemy(new Vector2(pos.x, pos.y));
        moveTo = new Vector2(pos.x, pos.y);
    }

    public void InstantiateEnemy(Vector2 position)
    {
        Transform instance;

        Transform healthTextFab = 
            (Transform)Resources.Load(
                "enemy_text", typeof(Transform));

        //Instantiate Enemy 
        instance = MonoBehaviour
            .Instantiate(prefab, position, Quaternion.identity);
        anim = instance.GetComponent<Animator>();
        enemyGUI.Add(instance);

        //Instantiate Enemy Health Bar
        instance = MonoBehaviour
            .Instantiate((Transform)Resources.Load(
                "healthbar_sprite", typeof(Transform)),
                position + new Vector2(0f, 1.8f), Quaternion.identity);

        enemyGUI.Add(instance);


        //Instantiate Text
        instance = MonoBehaviour.Instantiate(healthTextFab,
            position + new Vector2(0f, 1.8f), Quaternion.identity);
        textBox = instance.GetComponent<TextMesh>();
        textBox.text = Utils.ToDisplayText(health);
        enemyGUI.Add(instance);

        instances = enemyGUI;
        enemyGUI = new List<Transform> { };
    }

    public void RegisterDeathObserver(IDeathObserver observer)
    {
        deathObservers.Add(observer);
    }


    //this shouldnt even exist.  The stage class has the buffer width
    private float GetEffectiveWidth() 
    {
        //3 is the buffer
        return width;
    }

    public void UnregisterDeathObserver(IDeathObserver observer)
    {
        deathObservers.Remove(observer);
    }

    public void Move(Vector2 position)
    {
        if (isFixed || !isActive)
        {
            return;
        }
        if (moveTo.x != position.x || moveTo.y != position.y)
        {
            moveTo = position;
            if (!isMoving)
            {
                isMoving = true;
                IEnumerator coroutine = MoveEnemy();
                gameController.StartCoroutine(coroutine);
           }
        }

    }

    //Moves enemy to it moveTo
    IEnumerator MoveEnemy()
    {
        if (!CheckDead())
        {
            anim.SetBool("Walk", true);

            //Get End position of everything
            List<Vector3> startPositions = new List<Vector3> { };
            foreach (Transform i in instances)
            {
                startPositions.Add(i.position);
            }

            while (instances[0].position.x > moveTo.x)
            {
                //Use the coded out one for dynamic move speed
                //instances.ForEach(i => i.position += new Vector3((moveTo.x - startPositions[instances.IndexOf(i)].x) * Time.deltaTime * walkSpeed, 0, 0));
                instances.ForEach(i => 
                i.position -= new Vector3(Time.deltaTime * walkSpeed, 0, 0));

                yield return null;
            }

            instances.ForEach(i =>
            i.position = new Vector2(moveTo.x, i.position.y));
            anim.SetBool("Idle", true);
            isMoving = false;
        }
    }

    public override void TakeDamage(float baseDamage)
    {
        VictoryStats.damageDone += inAdditive + (inMultiplier * baseDamage); //Won't work if there's another forumla for damagetaken
        base.TakeDamage(baseDamage);
    }

}



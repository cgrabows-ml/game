using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public abstract class Enemy : Character
{
    public string name;
    private List<IDeathObserver> deathObservers = new List<IDeathObserver>();

    public float width;
    public Boolean isFixed = false;
    public Boolean isActive = false;
    public Boolean hasCollision = true;
    public Boolean isDying = false;

    /// <summary>
    /// Constructor for enemy class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    public Enemy(string name,String prefabPath,
        float health = 100, float width = 1,
        float maxGCD = 2)
        : base(prefabPath, health)
    {
        this.width = width;
        this.name = name;
    }

    new public virtual void Update()
    {
        if (isActive)
        {
            base.Update();
            Cast();
        }

    }

    /// <summary>
    /// This is the AI for the enemy.The enemy goes through his spellbook
    /// and sees if it's valid for him to cast.
    /// This also triggers the enemy GCD, and animates the enemy.
    /// </summary>
    public virtual void Cast()
    {
        if (isActive)
        {
            spellbook.ForEach(spell => CastIfAble(spell));
        }
    }

    public override void InstantiateCharacter(Vector2 position)
    {
        base.InstantiateCharacter(position);

        if (GameController.gameController.stage.inCombat)
        {
            anim.SetBool("Entrance", true);
            isActive = false;
        }
    }

    public override void CheckDeadAndKill()
    {
        if (CheckDead())
        {
            isActive = false;
            //Add to total kill number
            VictoryStats.enemiesSlain += 1;

            //Move enemy and uninstantiate
            anim.SetBool("Death", true);
            moveTo = sprite.position;
            deathObservers.ForEach(observer => {
                observer.DeathUpdate(this);});
            isDying = true;
            UpdateStatusBars();
            IEnumerator coroutine = DestroyAfterTime(deathTime);
            gameController.StartCoroutine(coroutine);
        }

    }

    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);
        isActive = true;
        moveTo = new Vector2(pos.x, pos.y);
    }



    IEnumerator SetActive()
    {
        float time = 0;
        while(time <= 1)
        {
            //Only updates time to become active if youre in combat
            if (gameController.stage.inCombat)
            {
                time += Time.deltaTime;
            }
            yield return null;
        }
        isActive = true;
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

    public override void Move(Vector2 position)
    {
        if (isFixed || !isActive)
        {
            return;
        }
        base.Move(position);
    }

    public override float TakeDamage(float baseDamage, Character source)
    {
        VictoryStats.damageDone += inAdditive + (inMultiplier * baseDamage); //Won't work if there's another forumla for damagetaken
        return base.TakeDamage(baseDamage, source);
    }

}



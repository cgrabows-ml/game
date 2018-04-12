using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage: IDeathObserver {

    public GameController gameController = GameController.gameController;
    public Hero hero;
    public List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> movableEnemies = new List<Enemy>();
    public List<Enemy> fixedEnemies = new List<Enemy>();
    public Boolean inCombat = false;
    public int numEncounter = 0;
    public int damageDone;

    private List<Encounter> encounters;
    private float leftMostPositionX = -1;
    private float rightScreenEdgePositionX = 6.58f;
    private Boolean canProceed = false;

    private Vector3 heroPosition = new Vector3(-10,-2.58f,0); //replace me
    private float bufferWidth = .4f; //replace me
    public float groundY = -2.58f; //replace me
    private float startingRun;

    public Stage()
    {
    }

    public void SetEncounters(List<Encounter> encounters)
    {
        this.encounters = encounters;
    }

    private void SetHero()
    {
        hero = new Hero();
        hero.Spawn(heroPosition);
        hero.MoveRight(heroPosition);
    }

    public void StartStage () {
        SetHero();
        StartNextEncounter();
        inCombat = false;
        IEnumerator coroutine = SetActive();
        gameController.StartCoroutine(coroutine);
	}

    IEnumerator SetActive()
    {
        float time = 0;
        float duration = 2;
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        inCombat = true;
    }

    public void EndStage()
    {
        SceneManager.LoadScene("Victory");
        inCombat = false;
    }

    public void Update()
    {
        if (inCombat)
        {
            enemies.ForEach(enemy => enemy.Update());
        }

        if (canProceed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameController.spaceContinue.gameObject.SetActive(false);
                StartNextEncounter2();
                canProceed = false;
            }
        }

    }

    public void EndEncounter()
    {
        inCombat = false;
        encounters.Remove(encounters[0]);
        //hero.spellbook.ForEach(i => i.numEncounter++);
        numEncounter++;

        IEnumerator coroutine = SetProceed();
        gameController.StartCoroutine(coroutine);

    }

    IEnumerator SetProceed()
    {
        yield return new WaitForSeconds(2);
        gameController.spaceContinue.gameObject.SetActive(true);
        canProceed = true;
    }

    public void DeathUpdate(Character character)
    {
        gameController.StartCoroutine(HandleEnemyDeath(character));
    }

    IEnumerator HandleEnemyDeath(Character character)
    {
        yield return new WaitForSeconds(character.deathTime - .1f);
        RemoveEnemy((Enemy)character);
        if (enemies.Count == 0)
            EndEncounter();
        MoveEnemies();
    }

    public List<Enemy> getActiveEnemies()
    {
        List<Enemy> result = new List<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            if (enemy.isActive)
            {
                result.Add(enemy);
            }
        }
        return result;
    }

    public void StartNextEncounter2()
    {
        if (encounters.Count > 0)
        {
            heroPosition = new Vector3(hero.instances[0].position.x, -2.58f, 0);
            leftMostPositionX += 6.09f;
            hero.MoveRight(heroPosition);
            encounters[0].StartEncounter();
            IEnumerator coroutine = SetActive();
            gameController.StartCoroutine(coroutine);
        }
        else
        {
            EndStage();
        }
    }

    public void StartNextEncounter()
    {
        if (encounters.Count > 0)
        {
            encounters[0].StartEncounter();
        }
        else
        {
            EndStage();
        }
    }

    //Removes enemy from enemy List
    public void RemoveEnemy(Enemy enemy)
    {
        int index = enemies.IndexOf(enemy);
        enemies.Remove(enemy);
    }

    IEnumerator MoveAfter(Enemy deadEnemy, float time)
    {
        //yield return new WaitForSeconds(2);
        //MoveEnemies(deadEnemy);
        yield return null;
    }

    //Adds enemy to enemy list and instantiates
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        SetupEnemy(enemy);

        //SpawnEnemyOffscreen(enemy);
    }

    public void SetupEnemy(Enemy enemy)
    {
        enemy.RegisterDeathObserver(this);
        //enemy.Spawn(new Vector2(getNextFreeSpace(enemy, leftMostPositionX), groundY));
        Vector2 position;
        if (enemy.isFixed)
        {
            fixedEnemies.Add(enemy);
            position = enemy.moveTo;
        }
        else
        {
            movableEnemies.Add(enemy);
            float nextX = getNextFreeSpace(enemy, leftMostPositionX);
            position = new Vector2(nextX, groundY);
        }
        enemy.Spawn(position);
    }

    public void AddEnemyAtIndex(Enemy enemy, int index)
    {
        enemies.Insert(index, enemy);
        SetupEnemy(enemy);
    }

    ///// <summary>
    ///// Spawn enemy offscreen and have it walk into the correct position
    ///// </summary>
    ///// <param name="enemy"></param>
    //public void SpawnEnemyOffscreen(Enemy enemy)
    //{
    //    float effectiveWidth = enemy.width + bufferWidth;
    //    //MonoBehaviour.print(rightScreenEdgePositionX + " " + effectiveWidth);
    //    enemy.Spawn(new Vector2(rightScreenEdgePositionX + effectiveWidth, 0));
    //    //MoveEnemies();
    //}

    public void SetEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
        enemies.ForEach(enemy => SetupEnemy(enemy));
    }

    /// <summary>
    /// Spawns enemies offscreen, in correct position and then has them walk onscreen.
    /// </summary>
    /// <param name="enemies"></param>
    public void SpawnEnemiesOffscreen(List<Enemy> enemies)
    {
        //Spawn enemies in order off screen
        foreach (Enemy enemy in enemies)
        {
            // move enemy to correct position
            float nextPositionX = getNextFreeSpace(enemy, rightScreenEdgePositionX);
            Vector2 spawnPos = new Vector2(nextPositionX, groundY);
            enemy.Spawn(spawnPos);
        }
    }

    /// <summary>
    /// Gets x coordinate of the next free space.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float getNextFreeSpace(Enemy target, float startPos)
    {
        float x = startPos;
        int targetIndex = enemies.IndexOf(target);
        for(int i = 0; i < enemies.Count; i++)
        {
            if (i != targetIndex)
            {
                Enemy enemy = enemies[i];
                if (enemy.hasCollision &&
                    (i < targetIndex || enemy.isFixed))
                {
                    float targetLeft = x;
                    float targetRight = x + target.width + bufferWidth;
                    float enemyLeft = enemy.moveTo.x;
                    float enemyRight = enemyLeft + enemy.width + bufferWidth;
                    Boolean collision;
                    if (targetLeft > enemyLeft)
                    {
                        collision = targetLeft < enemyRight;
                    }
                    else
                    {
                        collision = enemyLeft < targetRight;
                    }
                    if (collision)
                    {
                        x = targetRight;
                    }
                }
            }
        }
        return x;
    }

    //meant to be for moving enemies when they see an open spot upon death
    //Current implementation is inefficient, but should be sufficient.
    public void MoveEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            if (!enemy.isFixed && enemy.isActive)
            {
                float nextX = getNextFreeSpace(enemy, leftMostPositionX);
                Vector2 position = new Vector2(nextX, groundY);
                enemy.Move(position);
                //MonoBehaviour.print(String.Format("Move {0} to {1}", enemy.name, position.x));
            }
        }
        //sort the enemy list by x coordinate
        enemies.Sort(delegate (Enemy e1, Enemy e2) 
        { return e1.moveTo.x.CompareTo(e2.moveTo.x); });
    }
}

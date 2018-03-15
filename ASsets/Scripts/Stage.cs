using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {

    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    public Hero hero;
    public List<Enemy> enemies = new List<Enemy>();
    public Boolean inCombat = false;

    private List<Encounter> encounters;
    private float leftMostPositionX = 0;
    private float rightScreenEdgePositionX = 6.58f;
    private Boolean canProceed = false;

    private Vector3 heroPosition = new Vector3(-10,-2.58f,0); //replace me
    private float bufferWidth = 1; //replace me
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
        hero = new Hero(playerController.heroHealthText);
        hero.Spawn(heroPosition);
        hero.MoveRight(heroPosition);
    }

    public void StartStage () {
        SetHero();
        StartNextEncounter();
        inCombat = false;
        IEnumerator coroutine = SetActive();
        playerController.StartCoroutine(coroutine);
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
        MonoBehaviour.print("Stage Done");
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
                playerController.spaceContinue.gameObject.SetActive(false);
                StartNextEncounter2();
                canProceed = false;
            }
        }

    }

    public void EndEncounter()
    {
        inCombat = false;
        encounters.Remove(encounters[0]);
        hero.spellbook.ForEach(i => i.numEncounter++);

        IEnumerator coroutine = SetProceed();
        playerController.StartCoroutine(coroutine);

    }

    IEnumerator SetProceed()
    {
        yield return new WaitForSeconds(2);
        playerController.spaceContinue.gameObject.SetActive(true);
        canProceed = true;
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
            playerController.StartCoroutine(coroutine);
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
        MoveEnemies(enemy, index);
        IEnumerator coroutine = enemy.DestroyAfterTime(2);
        playerController.StartCoroutine(coroutine);
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
        SpawnEnemyOffscreen(enemy);
    }


    /// <summary>
    /// Spawn enemy offscreen and have it walk into the correct position
    /// </summary>
    /// <param name="enemy"></param>
    public void SpawnEnemyOffscreen(Enemy enemy)
    {
        float effectiveWidth = enemy.width + bufferWidth;
        //MonoBehaviour.print(rightScreenEdgePositionX + " " + effectiveWidth);
        enemy.Spawn(new Vector2(rightScreenEdgePositionX + effectiveWidth, 0));
        //MoveEnemies();
    }

    public void SetEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }

    /// <summary>
    /// Spawns enemies offscreen, in correct position and then has them walk onscreen.
    /// </summary>
    /// <param name="enemies"></param>
    public void SpawnEnemiesOffscreen(List<Enemy> enemies)
    {
        //Spawn enemies in order off screen
        float nextPositionX = rightScreenEdgePositionX;
        foreach (Enemy enemy in enemies)
        {
            // move enemy to correct position
            Vector2 spawnPos = new Vector2(nextPositionX, groundY);

            enemy.Spawn(spawnPos);
            nextPositionX += enemy.width / 2 + bufferWidth;

        }
        //MoveEnemies();
    }

    public void SpawnStartingEnemies()
    {
        float nextPositionX = leftMostPositionX;
        foreach (Enemy enemy in enemies)
        {
            Vector2 spawnPos = new Vector2(nextPositionX, groundY);

            enemy.Spawn(spawnPos);
            nextPositionX += enemy.width / 2 + bufferWidth;

        }
    }

    /*private void MoveEnemies()
    {
        float nextPositionX = leftMostPositionX;
        foreach (Enemy enemy in enemies)
        {
            enemy.Move(rightScreenEdgePositionX - leftMostPositionX);
        }

    }*/


    //meant to be for moving enemies when they see an open spot upon death
    private void MoveEnemies(Enemy deadEnemy, int index)
    {
        if(index < enemies.Count)
        {
            float nextPositionX = 0;
            Boolean toMove = false;

            while (index < enemies.Count)
            {
                //check if mid-
                if (enemies[index].moveTo.x == enemies[index].instances[0].position.x)
                {
                    toMove = true;
                }
                if (index == 0)
                {
                    enemies[0].moveTo = new Vector3(leftMostPositionX, groundY);
                    nextPositionX = enemies[index].width / 2 + bufferWidth;
                    if (toMove){enemies[index].Move(1);}
                }
                else
                {
                    enemies[index].moveTo = new Vector3(enemies[index - 1].moveTo.x + enemies[index - 1].width / 2 + bufferWidth, 0);
                    if (toMove){ enemies[index].Move(1);}
                }
                toMove = false;
                //nextPositionX += enemies[index].width / 2 + bufferWidth;
                index++;
            }
        }
    }
}

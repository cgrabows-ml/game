using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {

    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    public Hero hero;
    public List<Enemy> enemies = new List<Enemy>();

    private List<Encounter> encounters;
    private float leftMostPositionX = 0;
    private float rightScreenEdgePositionX = 6.58f;

    private Vector3 heroPosition; //replace me
    private float bufferWidth = 1; //replace me
    private float groundY = -2.58f; //replace me

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
    }

    public void StartStage () {
        SetHero();
        StartNextEncounter();
	}

    public void EndStage()
    {

    }

    public void Update()
    {
        enemies.ForEach(enemy => enemy.Update());
    }

    public void EndEncounter()
    {
        //Need to change overall state in some way to disable ability usage etc.
        //hero.inCombat = false; 
        //gameState = "walking";
    }

    public void StartNextEncounter()
    {
        if (encounters.Count > 0)
        {
            encounters[0].StartEncounter();
            encounters.Remove(encounters[0]);
        }
        else
        {
            EndStage();
        }
    }

    //Removes enemy from enemy List
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        MoveEnemies();
        IEnumerator coroutine = enemy.DestroyAfterTime(2);
        playerController.StartCoroutine(coroutine);
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
        MoveEnemies();
    }

    public void SetEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
        //MoveEnemies();
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
        MoveEnemies();
    }

    private void MoveEnemies()
    {
        float nextPositionX = leftMostPositionX;
        foreach (Enemy enemy in enemies)
        {
            enemy.Move(rightScreenEdgePositionX - leftMostPositionX);
        }
    }



}

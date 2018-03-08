using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {

    public Hero hero;
    public List<Enemy> enemies = new List<Enemy>();

    private List<Encounter> encounters;
    private int leftMostPositionX;
    private int rightScreenEdgePositionX;

    private Vector3 heroPosition; //replace me
    private int bufferWidth = 123; //replace me
    private int groundY = 123; //replace me

    public Stage()
    {
    }

    public void SetEncounters(List<Encounter> encounters)
    {
        this.encounters = encounters;
    }

    private void SetHero()
    {
        hero = new Hero(heroHealthText);
        hero.Spawn(heroPosition);

        //instantiate Hero

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
    public void RemoveEnemy(Enemy enemy, List<Transform> instances)
    {
        enemies.Remove(enemy);
        MoveEnemies();
        IEnumerator coroutine = DestroyAfterTime(2, instances);
        StartCoroutine(coroutine);
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
        int effectiveWidth = enemy.width + bufferWidth;
        enemy.Spawn(rightScreenEdgePositionX + effectiveWidth);
        MoveEnemies();
    }

    public void SetEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
        MoveEnemies();
    }

    /// <summary>
    /// Spawns enemies offscreen, in correct position and then has them walk onscreen.
    /// </summary>
    /// <param name="enemies"></param>
    public void SpawnEnemiesOffscreen(List<Enemy> enemies)
    {
        //Spawn enemies in order off screen
        int nextPositionX = rightScreenEdgePositionX;
        foreach (Enemy enemy in enemies)
        {
            // move enemy to correct position
            int effectiveWidth = enemy.width + bufferWidth;
            enemy.Spawn(nextPositionX + effectiveWidth / 2, groundY);
            nextPositionX += effectiveWidth;
        }
        MoveEnemies();
    }

    private void MoveEnemies()
    {
        int nextPositionX = leftMostPositionX;
        foreach (Enemy enemy in enemies)
        {
            // move enemy to correct position
            int effectiveWidth = enemy.width + bufferWidth;
            enemy.Move(nextPositionX + effectiveWidth / 2);
            nextPositionX += effectiveWidth;
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Encounter {

    public Boolean winCon = false;

    private Stage stage;
    private List<Enemy> startingEnemies;

    /// <summary>
    /// Constructor for Encounter class.
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="startingEnemies"></param>
    public Encounter(Stage stage)
    {
        this.stage = stage;
        this.startingEnemies = getStartingEnemies();
    }

    protected abstract List<Enemy> getStartingEnemies();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemies"></param>
    private void SetEnemies(List<Enemy> enemies)
    {
        stage.SetEnemies(enemies);
    }

	public void StartEncounter () {
        SetEnemies(startingEnemies);
	}

    public void Update()
    {
        

    }
}

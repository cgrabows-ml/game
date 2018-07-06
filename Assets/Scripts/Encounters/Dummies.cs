using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummies : Encounter
{
    private int numDummies;

    public Dummies(Stage stage, int numDummies)
        : base(stage)
    {
        this.numDummies = numDummies;
        stage.leftMostPositionX += 1f;
    }

    protected override List<Enemy> getStartingEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        for(int i=0; i<numDummies; i++)
        {
            enemies.Add(new Dummy());
        }
        return enemies;
    }

}
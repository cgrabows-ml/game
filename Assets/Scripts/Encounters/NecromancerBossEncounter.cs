using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBossEncounter : Encounter
{

    public NecromancerBossEncounter(Stage stage)
        : base(stage)
    {
        stage.leftMostPositionX = -2f;
    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new NecromancerBoss() };
    }

}
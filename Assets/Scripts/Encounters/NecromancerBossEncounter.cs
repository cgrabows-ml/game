using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBossEncounter : Encounter
{

    public NecromancerBossEncounter(Stage stage)
        : base(stage)
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new NecromancerBoss() };
    }

}
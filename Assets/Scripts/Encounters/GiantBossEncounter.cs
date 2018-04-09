using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBossEncounter : Encounter
{

    public GiantBossEncounter(Stage stage)
        : base(stage)
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new Giant() };
    }

}
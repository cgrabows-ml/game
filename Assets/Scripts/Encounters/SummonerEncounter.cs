using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerEncounter: Encounter
{

    public SummonerEncounter(Stage stage)
        : base(stage)
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() {};
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : Stage
{

    public TutorialStage()
    {
        //Encounter encounter = new Encounter(stage, enemies);
        List<Encounter> encounters = new List<Encounter>() {
            //new Dummies(this, 4),
            new SummonerEncounter(this)
        };

        //this.leftMostPositionX = -1f;
        SetEncounters(encounters);
    }
}


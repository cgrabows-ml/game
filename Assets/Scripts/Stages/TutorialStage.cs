using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : Stage
{

    public TutorialStage()
    {
        //Encounter encounter = new Encounter(stage, enemies);
        List<Encounter> encounters = new List<Encounter>() {
            new FourDummies(this),
            new FourDummies(this)
        };
        SetEncounters(encounters);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBossStage : Stage {

    public NecromancerBossStage()
        :base()
    {
        //Encounter encounter = new Encounter(stage, enemies);
        List<Encounter> encounters = new List<Encounter>() {
            new NecromancerBossEncounter(this) };
        SetEncounters(encounters);
    }
}

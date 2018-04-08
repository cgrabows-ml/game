using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter3 : Encounter
{


    public Encounter3(Stage stage)
        : base(stage)
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new Warrior(), new Warrior(),
            new Giant() };
    }

}
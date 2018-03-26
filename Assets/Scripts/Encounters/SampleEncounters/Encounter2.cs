using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter2 : Encounter
{


    public Encounter2(Stage stage)
        : base(stage)
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new Warrior(), new Warrior(),
            new Warrior() };
    }

}
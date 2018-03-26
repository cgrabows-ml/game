using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter1 : Encounter {


    public Encounter1(Stage stage)
        : base(stage) 
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new Necromancer() };
    }

}

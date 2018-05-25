using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDummies : Encounter
{

    public FourDummies(Stage stage)
        : base(stage)
    {

    }

    protected override List<Enemy> getStartingEnemies()
    {
        return new List<Enemy>() { new Dummy(), new Dummy(), new Dummy(), new Dummy() };
    }

}
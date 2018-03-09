using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter1 : Encounter {


    public Encounter1(Stage stage)
        :base(stage, new List<Enemy>() { new Warrior(new Vector3(0, 0, 0)), new Warrior(new Vector3(0, 0, 0)), new Warrior(new Vector3(0, 0, 0)) }) 
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Spell {

    public Slash(Character owner)
        :base()
    {
        this.owner = owner;
        this.baseCooldown = 5;
        this.baseDamage = 5;
        this.triggersGCD = true;
        this.GCDRespect = true;
        this.animationKey = "Use1";
        this.target = "front";
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCastObserver {

    // Update is called once per frame
    public abstract void Update(Spell spell, Character caster);
}

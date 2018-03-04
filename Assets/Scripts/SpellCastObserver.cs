using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SpellCastObserver {

    void SpellCastUpdate(Spell spell, Character caster);
}

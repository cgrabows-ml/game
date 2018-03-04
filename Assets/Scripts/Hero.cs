using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Character {

    /// <summary>
    /// Constructor for Hero class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    // Use this for initialization
    public Hero(List<Spell> spellbook, TextMesh textBox, Animator anim, float health = 100)
        :base(spellbook, textBox, anim, 100)
    {

	}
}

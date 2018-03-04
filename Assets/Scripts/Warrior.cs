using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : Enemy {

    public Warrior(TextMesh text, Animator anim)
        : base("warrior", 3, new List<Spell> { new DamageSpell(4, 1, "Use1", target: "player"), new DamageSpell(6, 5, "Use2", target: "player") }, text, anim, 120)
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Character
{

    /// <summary>
    /// Constructor for Hero class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    // Use this for initialization
    public Hero(TextMesh textBox)
        : base(new List<Spell> { new DamageSpell(3, 1, "Use1", delay:1), new DamageSpell(6, 2, "Use2", target: "back"), new DamageSpell(8, 3, "Use3", target: "AoE"), new Empower() },
        (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/blackKnight.prefab", typeof(Transform)),
            textBox,
            100)
    {
        Transform instance = MonoBehaviour.Instantiate(prefab);
        instances.Add(instance);
        anim = instance.GetComponent<Animator>();
        textBox.text = Utils.ToDisplayText(health);
    }
}
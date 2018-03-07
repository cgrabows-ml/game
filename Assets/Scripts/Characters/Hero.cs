using System;
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
        : base(new List<Spell> { new DamageSpell(3, 1, "Use1", delay:1), new Fireball(), new DamageSpell(8, 3, "Use3", target: "AoE"), new Empower() },
        (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/blackKnight.prefab", typeof(Transform)),
            textBox,
            20)
    {
        Transform instance = MonoBehaviour.Instantiate(prefab);
        instances.Add(instance);
        anim = instance.GetComponent<Animator>();
        textBox.text = Utils.ToDisplayText(health);
    }

    //Also casts the spell
    public override Boolean CastIfAble(Spell spell)
    {
        if (base.CastIfAble(spell))
        {
            int i = spellbook.IndexOf(spell);
            GameObject instance = MonoBehaviour.Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/CooldownCover.prefab", typeof(GameObject)));
            IEnumerator coroutine = CooldownCover(i, instance);
            playerController.StartCoroutine(coroutine);

            return true;
        }
        else
        {
            return false;
        }
    }


    public override void CheckDeadAndKill()
    {
        base.CheckDeadAndKill();
        if (health <= 0)
        {
            Time.timeScale = 0;
        }
    }

    IEnumerator CooldownCover(int index, GameObject instance)
    {
        float duration = spellbook[index].baseCooldown;
        float time = 0;
        Transform r = instance.GetComponent<Transform>();
        r.position += new Vector3(0 + .93f * index, Time.deltaTime / duration * .8f / 2, 0);
        while (time < duration)
        {
            time += Time.deltaTime;
            r.localScale -= new Vector3(0, Time.deltaTime / duration * .8f, 0);
            r.position -= new Vector3(0, Time.deltaTime / duration * .8f / 2, 0);
            yield return null;
        }
        MonoBehaviour.Destroy(instance.gameObject);
    }
}
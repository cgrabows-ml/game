using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Character
{

    List<RectTransform> castCovers = new List<RectTransform>{ };

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
            200)
    {

    }

    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);
        castCovers = new List<RectTransform> { playerController.castCover1, playerController.castCover2, playerController.castCover3, playerController.castCover4 };
    }

    //Also casts the spell
    public override Boolean CastIfAble(Spell spell)
    {
        if (base.CastIfAble(spell))
        {
            //Make cast cover "visible"
            int i = spellbook.IndexOf(spell);
            RectTransform cover = castCovers[i]; //throws error if there are more spells than casts on the screen

            cover.localScale = new Vector3(1,1,0);
           
            IEnumerator coroutine = CooldownCover(i, cover);
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

    IEnumerator CooldownCover(int index, Transform instance)
    {
        Vector3 basePos = instance.localPosition;
        float duration = spellbook[index].baseCooldown;
        float time = 0;
        Transform r = instance.GetComponent<Transform>();
        //r.position += new Vector3(0 + .93f * index, Time.deltaTime / duration * .8f / 2, 0);
        while (time < duration)
        {
            time += Time.deltaTime;
            r.localScale -= new Vector3(0, Time.deltaTime / duration, 0);
            r.localPosition -= new Vector3(0, Time.deltaTime / duration * 80 / 2, 0);
            yield return null;
        }
        r.localPosition = basePos;
        r.localScale = new Vector3(0, 0, 0);
    }
}
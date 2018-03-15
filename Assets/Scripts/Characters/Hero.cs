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
        : base(new List<Spell> { new DamageSpell(3, 1, "Use1"), new Fireball(), new DamageSpell(8, 3, "Use3", target: "AoE"), new Empower() },
        (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/blackKnight.prefab", typeof(Transform)),
            textBox,
            200)
    {
        maxGCD = 1;
    }

    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);

        //set camera
        playerController.cam.transform.position = new Vector3(pos.x + 3.91f, 0, -6); 

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

    public void MoveRight(Vector3 startingPosition)
    {
        IEnumerator coroutine = MoveRight2(startingPosition);
        playerController.StartCoroutine(coroutine);
    }

    IEnumerator MoveRight2(Vector3 startingPosition)
    {
        float time = 0;
        float walkTime = 2;

        anim.SetBool("Walk", true);

        Vector3 camStartPos = playerController.cam.transform.position;

        while (time < walkTime)
        {
            //Move hero
            instances[0].position += new Vector3(6.09f, 0,0) * Time.deltaTime / walkTime;

            //Move camera
            playerController.cam.transform.position += new Vector3(6.09f, 0, 0) * Time.deltaTime / walkTime;

            time += Time.deltaTime;
            yield return null;
        }
        instances[0].transform.position = new Vector3(startingPosition.x + 6.09f, -2.58f,0);
        playerController.cam.transform.position = new Vector3(camStartPos.x + 6.09f, 0, -6);
        anim.SetBool("Idle", true);

    }

    IEnumerator CooldownCover(int index, Transform instance)
    {
        Vector3 basePos = instance.localPosition;
        float duration = spellbook[index].baseCooldown;
        float time = 0;
        Transform r = instance.GetComponent<Transform>();
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
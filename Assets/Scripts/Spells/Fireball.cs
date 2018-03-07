using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fireball : DamageSpell
{

    public Transform fireball;

    public Fireball()
        : base(6, 2, "Use2", true, "front", true, 2)
    {

    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);


        //Create game object for Empower graphic
        Transform prefab = (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/fireball.prefab", typeof(Transform));
        fireball = MonoBehaviour.Instantiate(prefab);
        IEnumerator coroutine = MoveFireball();
        playerController.StartCoroutine(coroutine);

    }

    IEnumerator MoveFireball()
    {
        float time = 0;
        float duration = 2;
        while(time < duration)
        {
            fireball.position += new Vector3(1.2f * Time.deltaTime , 0, 0);

            time += Time.deltaTime;
            yield return null;
        }
        MonoBehaviour.Destroy(fireball.gameObject);
    }

}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lob : Projectile {

    public Lob(Vector3 startPos, Vector3 endPos, Transform prefab, float duration)
         : base(startPos, endPos, prefab, duration)
    {
       
    }

    public override IEnumerator MoveObject(Transform projectile, float duration)
    {
        float time = 0;
        projectile.position = startPos;

        float yVel = duration * 9.8f / 2;
        float xVel = (endPos.x - startPos.x) / duration;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);

        while (time < duration)
        {
            //rotates projectile assuming right end is front
            float angle = Mathf.Atan2(projectile.GetComponent<Rigidbody2D>().velocity.y, projectile.GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            time += Time.deltaTime;
            yield return null;

        }
        MonoBehaviour.Destroy(projectile.gameObject);

    }

}
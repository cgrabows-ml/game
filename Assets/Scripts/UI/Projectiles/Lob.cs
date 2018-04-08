using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lob : Projectile {

    public Lob(Vector3 endPos, Transform projectile, float duration)
         : base(endPos, projectile, duration)
    {
        float yVel = duration * 9.8f / 2;
        float xVel = (endPos.x - startPos.x) / duration;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
    }

    protected override void UpdatePosition()
    {
        //rotates projectile assuming right end is front
        float angle = Mathf.Atan2(projectile.GetComponent<Rigidbody2D>().velocity.y, projectile.GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}

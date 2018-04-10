using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile {
    
    protected Vector3 startPos;
    protected Vector3 endPos;
    protected float duration;
    protected Transform projectile;

    public Projectile(Vector3 endPos, Transform projectile,
        float duration)
    {
        this.startPos = projectile.position;
        this.endPos = endPos;
        this.duration = duration;
        this.projectile = projectile;

        //Transform projectile = MonoBehaviour.Instantiate(prefab);
        IEnumerator coroutine = MoveObject();
        GameController.gameController.StartCoroutine(coroutine);
    }

    protected virtual void FinishMoving()
    {
        MonoBehaviour.Destroy(projectile.gameObject);
    }
    
    public virtual IEnumerator MoveObject()
    {
        float time = 0;
        while (time < duration)
        {
            UpdatePosition();
            time += Time.deltaTime;
            yield return null;
        }
        FinishMoving();
    }

    protected virtual void UpdatePosition()
    {
        projectile.position += (endPos - startPos) *
                Time.deltaTime / duration;
    }
}

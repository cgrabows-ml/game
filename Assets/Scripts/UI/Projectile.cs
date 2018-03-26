using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile {
    
    protected Vector3 startPos;
    protected Vector3 endPos;

    public Projectile(Vector3 startPos, Vector3 endPos, Transform prefab,
        float duration)
    {
        this.startPos = startPos;
        this.endPos = endPos;

        Transform projectile = MonoBehaviour.Instantiate(prefab);
        IEnumerator coroutine = MoveObject(projectile, duration);
        GameController.gameController.StartCoroutine(coroutine);
    }

    
    public virtual IEnumerator MoveObject(Transform projectile,
        float duration)
    {
        float time = 0;
        projectile.position = startPos;
        while (time < duration)
        {
            projectile.position += (endPos - startPos) *
                Time.deltaTime / duration;

            time += Time.deltaTime;
            yield return null;
        }
        MonoBehaviour.Destroy(projectile.gameObject);
    }
}

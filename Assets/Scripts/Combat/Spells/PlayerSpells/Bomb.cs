using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : DamageSpell {

    private float finalDamage;

    public Bomb(Character caster)
        :base(caster, 3, 5, "Use2")
    {
        finalDamage = baseDamage;
    }

    public override void CastEffect()
    {
        IEnumerator coroutine = DamageAfterTime();
        gameController.StartCoroutine(coroutine);
    }

    IEnumerator DamageAfterTime()
    {
        int startNum = numEncounter;
        float finalDamage = caster.GetDamage(baseDamage);
        float time = 0;
        KeyCode bind = KeyCode.A;

        //Time before pressing the button again changes the effect
        while (time < 1)
        {
            time += Time.deltaTime;
            yield return null;
        }

        foreach (SpellBinding binding in gameController.spellBindings)
        {
            if (binding.spell == this)
            {
                bind = binding.GetKey();
            }
        }

        while (time < 2)
        {
            time += Time.deltaTime;
            
            if (Input.GetKeyDown(bind))
            {
                finalDamage = 10;
            }
            yield return null;
        }

        if (startNum == gameController.stage.numEncounter)
        {
            CombatUtils.DealDamage(caster, GetTargets(), finalDamage);
            finalDamage = baseDamage;
        }
    }

}

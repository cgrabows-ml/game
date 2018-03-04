using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : GameLogger {

    private float duration;
    protected Character owner;
    private String name;

    public Buff(String name, Character owner, int duration = 0)
    {
        this.duration = duration;
        this.owner = owner;
        this.name = name;
        ApplyBuff();
    }

    public String getName()
    {
        return name;
    }

    public virtual void ApplyBuff()
    {
        owner.buffs.Add(this);
    }

    public virtual void RemoveBuff()
    {
        owner.buffs.Remove(this);
    }

    /// <summary>
    /// Removes duration based on deltatime
    /// </summary>
    public void Update()
    {
        duration = Math.Max(duration - Time.deltaTime, 0);
        if (duration <= 0)
        {
            RemoveBuff();
        }
    }


}

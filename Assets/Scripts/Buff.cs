using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff {

    private float duration;
    protected Character owner;
    private String name;

    public Buff(String name, Character owner, int duration = 0)
    {
        this.duration = duration;
        this.owner = owner;
        this.name = name;
    }

    private Boolean CanRemove()
    {
        return duration <= 0;
    }

    public String getName()
    {
        return name;
    }

    public abstract void ApplyBuff();
    public abstract void RemoveBuff();

    /// <summary>
    /// Removes duration based on deltatime
    /// </summary>
    public void Update()
    {
        duration = Math.Max(duration - Time.deltaTime, 0);
        if (CanRemove())
        {
            RemoveBuff();
        }
    }


}

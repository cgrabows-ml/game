using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSummonSkeleton : Spell, IDeathObserver
{

    private Stage stage;
    private int skeletonsAlive = 0;
    private int totalLevel = 4;

    System.Random rand = new System.Random();


    public MassSummonSkeleton()
        : base(1, "Use2")
    {
        stage = gameController.stage;

    }

    public override void ReduceCooldown()
    {
        if (skeletonsAlive == 0)
        {
            cooldown = Math.Max(0, cooldown - Time.deltaTime);
        }
    }

    public void DeathUpdate(Character character)
    {
        skeletonsAlive -= 1;
        MonoBehaviour.print(skeletonsAlive);
    }

    private List<int> GetLevels()
    {
        int combinedLevels = 0;
        List<int> levels = new List<int>() { 1 };
        while(combinedLevels < totalLevel)
        {
            int level = 1;
            int num = rand.Next(0, 100);
            if(num > 90)
            {
                level = 4;
            }
            else if (num > 70)
            {
                level = 3;
            }
            else if (num > 40)
            {
                level = 2;
            }
            level = Math.Min(level, totalLevel - combinedLevels);
            combinedLevels += level;
            levels.Add(level);
        }
        return levels;
    }

    public override Boolean isCastable(Character caster)
    {
        return base.isCastable(caster) && skeletonsAlive == 0;
    }

    public override void Cast(Character caster)
    {
        base.Cast(caster);
        foreach(int level in GetLevels())
        {
            MonoBehaviour.print("level: " + level);
            int casterIndex = stage.enemies.IndexOf((Enemy)caster);
            Enemy skeleton = new Skeleton(1);
            stage.AddEnemyAtIndex(skeleton, casterIndex);
            MonoBehaviour.print("count "+ skeleton.deathObservers.Count);
            skeleton.RegisterDeathObserver(this);
            MonoBehaviour.print("count " + skeleton.deathObservers.Count);
            skeletonsAlive += 1;
            MonoBehaviour.print(skeletonsAlive);
        }   
    }
}
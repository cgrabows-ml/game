using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSummonSkeleton : Spell, IDeathObserver
{

    private Stage stage;
    private int skeletonsAlive = 0;
    private int totalLevel;
    private int minSkeletons;
    private int maxSkeletons;
    private int levelGainPerCast;

    System.Random rand = new System.Random();

    public MassSummonSkeleton(Character caster, int minSkeletons=4,
        int maxSkeletons=4, int totalLevel=4, int levelGainPerCast=2)
        : base(caster, 5f, "Use2", GCDRespect:false)
    {
        this.levelGainPerCast = levelGainPerCast;
        this.minSkeletons = minSkeletons;
        this.maxSkeletons = maxSkeletons;
        this.totalLevel = totalLevel;
        stage = gameController.stage;
        name = "Mass Summon Skeletons";
    }

    public override void ReduceCooldown()
    {
        if (skeletonsAlive == 0)
        {
            SetCooldown(Math.Max(0, GetCooldown() - Time.deltaTime));
        }
    }

    public void DeathUpdate(Character character)
    {
        skeletonsAlive -= 1;
    }

    private List<int> GetLevels()
    {
        int combinedLevels = 0;
        List<int> levels = new List<int>() { };
        while(combinedLevels < totalLevel)
        {
            int level = 1;
            int num = rand.Next(0, 100);
            if (num > 90)
            {
                level += 3;
            }
            else if (num > 70)
            {
                level += 2;
            }
            else if (num > 40)
            {
                level += 1;
            }
            int guarunteedLevels = minSkeletons - levels.Count - 1;
            level = Math.Min(level, totalLevel - combinedLevels - guarunteedLevels);
            //MonoBehaviour.print(guarunteedLevels);
            if (levels.Count == maxSkeletons - 1)
            {
                level = totalLevel - combinedLevels;
            }
            combinedLevels += (level);
            levels.Add(level);
        }
        return levels;
    }

    public override Boolean isCastable()
    {
        return base.isCastable() && skeletonsAlive == 0;
    }

    public override void CastEffect()
    {
        foreach(int level in GetLevels())
        {
            //MonoBehaviour.print("level: " + level);
            int casterIndex = stage.enemies.IndexOf((Enemy)caster);
            //Enemy skeleton = new Skeleton(level);
            Enemy skeleton = new Skeleton(level);
            stage.AddEnemyAtIndex(skeleton, casterIndex);
            //MonoBehaviour.print("count "+ skeleton.deathObservers.Count);
            skeleton.RegisterDeathObserver(this);
            //MonoBehaviour.print("count " + skeleton.deathObservers.Count);
            skeletonsAlive += 1;
            //MonoBehaviour.print(skeletonsAlive);
        }
        totalLevel += levelGainPerCast;
    }
}
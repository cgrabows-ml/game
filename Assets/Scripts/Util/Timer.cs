using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Timer
{

    private uint counter = 0;
    private Dictionary<uint, List<Action>> eventsToFire = new Dictionary<uint, List<Action>>();
    /// <summary>
    /// Increment the global frame counter, and call any functions scheduled at the new frame
    /// </summary>
    public void Increment()
    {
        counter++;
        // MonoBehaviour.print(counter);
        if (eventsToFire.ContainsKey(counter)){
            eventsToFire[counter].ForEach(func => func());
            eventsToFire.Remove(counter);
        }
    }

    /// <summary>
    /// Set a function to execute after (delay) frames.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="func"></param>
    public void SetTimer(uint delay, Action func)
    {
        uint currentTime = counter;
        uint timeToRun = currentTime + delay;
        if (eventsToFire.ContainsKey(timeToRun))
        {
            eventsToFire[timeToRun].Add(func);
        }
        else {
            eventsToFire.Add(timeToRun, new List<Action> { func });
        }
        
    }
}
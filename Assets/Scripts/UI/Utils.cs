using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils {

    /// <summary>
    /// A function that gets the ceiling of a value, then converts it to a string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The string of the ceiling of the input.</returns>
    public static String ToDisplayText(float value) //TODO move to general utils class or textBox wrapper class
    {
        return Math.Ceiling(value).ToString();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogger
{

    protected GamePrinter printer = GameObject.Find("PlayerController").GetComponent<GamePrinter>();
    private string logLevel = "debug";

    private Dictionary<string, int> logLevels = new Dictionary<string, int>() {{"fatal", 0}, {"error", 1}, {"warn", 2}, {"info", 3},
        {"debug", 4}, {"trace", 5}};

    private void log(string message, string messageLevel)
    {
        if (logLevels[messageLevel] >= logLevels[logLevel])
        {
            String className = this.GetType().Name;
            String time = DateTime.Now.ToString("h:mm:ss tt");
            String logMessage = String.Format("{0} - {1}: {2}", time, className, message);
            printer.gamePrint(logMessage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public void fatal(string message)
    {
        log(message, "fatal");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public void error(string message)
    {
        log(message, "error");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public void warn(string message)
    {
        log(message, "warn");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public void info(string message)
    {
        log(message, "info");
    }

    public void debug(string message)
    {
        log(message, "debug");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public void trace(string message)
    {
        log(message, "trace");
    }
}

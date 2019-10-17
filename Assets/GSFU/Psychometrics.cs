using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Psychometrics : MonoBehaviour
{
    private static ArrayList metrics = new ArrayList();
    private static string sessionID = "";
    private static string temp = "";
    public static string id = "";

    // Use this for initialization
    void Start()
    {
        if (sessionID.Equals(""))
        {
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;
            sessionID = (month + "-" + day + "-" + year + "-" + hour + "-" + minute + "-" + second);
            logEvent(sessionID);
        }
    }

    public static void attempt(string str)
    {
        temp = str;
    }

    public static void report(string str)
    {
        logEvent(str + " " + temp);
        temp = "";
    }

    public static void logEvent(string str)
    {
        metrics.Add(str);
    }

    public static void sendData()
    {
        while (metrics.Count > 255)
        {
            metrics.RemoveAt(metrics.Count - 1);
        }
        UnityDataConnector.instance.SendDataToSheet((string[])metrics.ToArray(typeof(string)));
        metrics = new ArrayList();
        logEvent(sessionID);
        logEvent(id);
    }
    
}

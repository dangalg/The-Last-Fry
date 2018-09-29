using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler{

    public static void SaveStringToDB(string name, string data){
        PlayerPrefs.SetString(name, data);
    }

    public static string LoadStringFromDB(string name){
        return PlayerPrefs.GetString(name);
    }

    public static void SaveIntToDB(string name, int data)
    {
        PlayerPrefs.SetInt(name, data);
    }

    public static int LoadIntFromDB(string name)
    {
        return PlayerPrefs.GetInt(name);
    }

    public static void SaveFloatToDB(string name, float data)
    {
        PlayerPrefs.SetFloat(name, data);
    }

    public static float LoadFloatFromDB(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }
}

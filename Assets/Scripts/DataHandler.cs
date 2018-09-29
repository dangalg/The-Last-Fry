using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataHandler{


    public static void SavePlayerData(PlayerData playerData){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        bf.Serialize(file, playerData);
        file.Close();
    }

    public static PlayerData LoadPlayerData(){

        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData playerData = (PlayerData)bf.Deserialize(file);
            file.Close();

            return playerData;

        }

        return new PlayerData();
    }

    public static void SaveStringToDB(string name, string data){
        PlayerPrefs.SetString(name, data);
        PlayerPrefs.Save();
    }

    public static string LoadStringFromDB(string name){
        return PlayerPrefs.GetString(name);
    }

    public static void SaveIntToDB(string name, int data)
    {
        PlayerPrefs.SetInt(name, data);
        PlayerPrefs.Save();
    }

    public static int LoadIntFromDB(string name)
    {
        return PlayerPrefs.GetInt(name);
    }

    public static void SaveFloatToDB(string name, float data)
    {
        PlayerPrefs.SetFloat(name, data);
        PlayerPrefs.Save();
    }

    public static float LoadFloatFromDB(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }


}

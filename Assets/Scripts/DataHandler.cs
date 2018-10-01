using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace TheLastFry
{
    public static class DataHandler
    {


        public static void SavePlayerData(PlayerData playerData)
        {

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

                bf.Serialize(file, playerData);
                file.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Couldn't save player data: " + ex.Message);
            }


        }

        public static PlayerData LoadPlayerData()
        {

            try
            {
                if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
                    PlayerData playerData = (PlayerData)bf.Deserialize(file);
                    file.Close();

                    return playerData;

                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Couldn't load player data: " + ex.Message);
            }


            return new PlayerData();
        }

        public static void SaveStringToDB(string name, string data)
        {
            PlayerPrefs.SetString(name, data);
            PlayerPrefs.Save();
        }

        public static string LoadStringFromDB(string name)
        {
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
}

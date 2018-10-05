using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace TheLastFry
{
    public static class DataHandler
    {

        /// <summary>
        /// Saves the player data.
        /// </summary>
        /// <param name="playerData">Player data.</param>
        public static void SavePlayerData(PlayerData playerData)
        {

            try
            {
                // create the formatter
                BinaryFormatter bf = new BinaryFormatter();

                // open or create the file
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

                // save the player data to the file
                bf.Serialize(file, playerData);

                // close the file
                file.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Couldn't save player data: " + ex.Message);
            }


        }

        /// <summary>
        /// Loads the player data.
        /// </summary>
        /// <returns>The player data.</returns>
        public static PlayerData LoadPlayerData()
        {

            try
            {
                // open the file if it exists
                if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
                {
                    // create the formatter
                    BinaryFormatter bf = new BinaryFormatter();

                    // open the file
                    FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

                    // get player data from file
                    PlayerData playerData = (PlayerData)bf.Deserialize(file);

                    // close the file
                    file.Close();

                    // return player data
                    return playerData;

                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Couldn't load player data: " + ex.Message);
            }


            return new PlayerData();
        }

        #region playerprefs
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

        #endregion playerprefs


    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheLastFry
{
    public class MainMenu : MonoBehaviour
    {

        public static MainMenu instance = null;

        public PlayerData playerData;

        [SerializeField] TMP_Text Level;
        [SerializeField] TMP_Text Energy;

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);

            //Call the InitGame function to initialize the first level 
            InitMenu();
        }

        void InitMenu()
        {
            playerData = DataHandler.LoadPlayerData();
        }

        // Use this for initialization
        void Start()
        {
            Level.text = playerData.Level.ToString();
            Energy.text = playerData.Energy.ToString();

            AdManager.instance.onFinishedAd = onFinishedAd;
        }

        public void DecreaseEnergy(int amount)
        {
            playerData.Energy -= amount;
            savePlayerData();
        }

        public void IncreaseEnergy(int amount)
        {
            playerData.Energy += amount;
            savePlayerData();
        }

        void savePlayerData()
        {
            DataHandler.SavePlayerData(playerData);
        }

        public void LoadGame()
        {
            if (playerData.RemoveAds || playerData.Energy > 0)
            {

                DecreaseEnergy(1);
                StartGame();
            }
            else
            {

                AdManager.instance.ShowAd();
            }

        }

        void onFinishedAd(){
            playerData.Energy = 3;
            playerData.Life = 3;
            StartGame();
        }

        void StartGame(){

            playerData.Level = 1;
            DataHandler.SavePlayerData(playerData);
            SceneManager.LoadScene("Game");
        }
    }
}

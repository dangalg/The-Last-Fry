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

        // player data
        public PlayerData playerData;

        // the texts on screen
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

        /// <summary>
        /// Inits the menu.
        /// </summary>
        void InitMenu()
        {
            // load player data
            playerData = DataHandler.LoadPlayerData();
        }

        // Use this for initialization
        void Start()
        {
            // display texts on screen
            Level.text = playerData.Level.ToString();
            Energy.text = playerData.Energy.ToString();

            // sign in for callback from ad manager
            AdManager.instance.onFinishedAd = onFinishedAd;
        }

        /// <summary>
        /// Decreases the energy.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public void DecreaseEnergy(int amount)
        {
            playerData.Energy -= amount;
            savePlayerData();
        }

        /// <summary>
        /// Increases the energy.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public void IncreaseEnergy(int amount)
        {
            playerData.Energy += amount;
            savePlayerData();
        }

        /// <summary>
        /// Saves the player data.
        /// </summary>
        void savePlayerData()
        {
            DataHandler.SavePlayerData(playerData);
        }

        /// <summary>
        /// Loads the game.
        /// </summary>
        public void LoadGame()
        {
            // if player paid for remove ads or the player energy is above zero
            if (playerData.RemoveAds || playerData.Energy > 0)
            {
                // remove one energy point
                DecreaseEnergy(1);

                // start game
                StartGame();
            }
            else
            {
                // show ad
                AdManager.instance.ShowAd();
            }

        }

        /// <summary>
        /// On finished watching ad.
        /// </summary>
        void onFinishedAd()
        {

            // refill player energy
            playerData.Energy = 3;

            // refill life
            playerData.Life = 3;

            // start game
            StartGame();
        }

        void StartGame()
        {
            // set player to level 1
            playerData.Level = 1;

            // save player data
            DataHandler.SavePlayerData(playerData);

            // load game
            SceneManager.LoadScene("Game");
        }
    }
}

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
        [SerializeField] TMP_Text Message;

        // buttons on screen
        [SerializeField] Button startButton;

        // panels on screen
        [SerializeField] GameObject MessagePanel;
        [SerializeField] GameObject InstructionsPanel;

        [SerializeField] GameObject ShopPanel;

        [SerializeField] GameObject ItemStorePanel;

        [SerializeField] GameObject LoadingPanel;

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

            DataHandler.SaveIntToDB("hand", 1);
            DataHandler.SaveIntToDB("black", 1);
            DataHandler.SaveIntToDB("chinese", 1);
        }

        // Use this for initialization
        void Start()
        {

            LoadingPanel.SetActive(false);

            // sign in for callback from ad manager
            AdManager.instance.onFinishedAd = onFinishedAd;

            ShopManager.instance.onRemoveAds = onRemoveAds;
            ShopManager.instance.onPurchaseFailedAction = onPurchaseFailed;

            SetupStartButton();

            // sign in to shopManager
            ShopManager.instance.onPurchaseCoinsAction += onPurchaseCoins;
            ShopManager.instance.onPurchaseFailedAction += onPurchaseFailed;
        }

        /// <summary>
        /// Ons the purchase coins.
        /// </summary>
        /// <param name="coinAmount">Coin amount.</param>
        void onPurchaseCoins(ShopManager.CoinAmount coinAmount)
        {
            // give gems to player TODO make exciting animation
            switch (coinAmount)
            {
                case ShopManager.CoinAmount.HANDFUL:
                    playerData.Coins += 500;
                    break;
                case ShopManager.CoinAmount.PILE:
                    playerData.Coins += 1200;
                    break;
                case ShopManager.CoinAmount.SACK:
                    playerData.Coins += 4000;
                    break;
                case ShopManager.CoinAmount.BAG:
                    playerData.Coins += 6500;
                    break;
                case ShopManager.CoinAmount.CHEST:
                    playerData.Coins += 10000;
                    break;
            }

            // save player data
            DataHandler.SavePlayerData(playerData);

            // hide shop panel
            ShopPanel.SetActive(false);

            // notify user of something wrong with purchase
            Message.text = "Thank You For The Purchase!";
            MessagePanel.SetActive(true);
        }

        /// <summary>
        /// Shows the instructions.
        /// </summary>
        public void ShowInstructions()
        {
            InstructionsPanel.SetActive(true);
        }

        /// <summary>
        /// Hides the instructions panel.
        /// </summary>
        public void HideInstructionsPanel()
        {
            // close Instructions Panel
            InstructionsPanel.SetActive(false);
        }

        /// <summary>
        /// Shows the item store.
        /// </summary>
        public void ShowItemStore()
        {
            ItemStorePanel.SetActive(true);
        }

        /// <summary>
        /// Hides the item store.
        /// </summary>
        public void HideItemStore()
        {
            // close Instructions Panel
            ItemStorePanel.SetActive(false);
        }

        /// <summary>
        /// Hides the message panel.
        /// </summary>
        public void HideMessagePanel()
        {
            // close Instructions Panel
            MessagePanel.SetActive(false);
        }

        /// <summary>
        /// Shows the item store.
        /// </summary>
        public void ShowShop()
        {
            ShopPanel.SetActive(true);
        }

        /// <summary>
        /// Hides the item store.
        /// </summary>
        public void HideShop()
        {
            // close Instructions Panel
            ShopPanel.SetActive(false);
        }

        /// <summary>
        /// Ons the remove ads.
        /// </summary>
        void onRemoveAds()
        {
            // thank user for his purchase
            Message.text = "Thank You For Your Purchase!";
            MessagePanel.SetActive(true);
        }

        /// <summary>
        /// Clicks the message panel.
        /// </summary>
        public void clickMessagePanel()
        {
            // close message panel
            MessagePanel.SetActive(false);
        }

        /// <summary>
        /// Ons the purchase failed.
        /// </summary>
        void onPurchaseFailed(){

            // notify user of something wrong with purchase
            Message.text = "Something Went Wrong Please Try Again Later..";
            MessagePanel.SetActive(true);
        }

        void SetupStartButton()
        {

            // start game
            startButton.GetComponentInChildren<Text>().text = "Start";

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
            // start game
            StartGame();
        }

        /// <summary>
        /// On finished watching ad.
        /// </summary>
        void onFinishedAd()
        {

            // refill player energy
            playerData.Energy = 3;

            // start game
            StartGame();
        }

        void StartGame()
        {

            LoadingPanel.SetActive(true);

            // set player to level 1
            playerData.Level = 1;

            // refill life
            playerData.Life = 3;

            // save player data
            DataHandler.SavePlayerData(playerData);

            // load game
            SceneManager.LoadScene("Game");
        }
    }
}

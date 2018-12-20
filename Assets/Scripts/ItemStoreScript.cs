using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheLastFry
{
    public class ItemStoreScript : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI coinsText;

        [SerializeField] ShopManager ShopPanel;

        // Use this for initialization
        void Start()
        {
        

            ShopPanel.onAfterPurchaseCoinsAction = onAfterPurchaseCoins;

            coinsText.text = MainMenu.instance.playerData.Coins.ToString();

        }

        void onAfterPurchaseCoins(ShopManager.CoinAmount coinAmount)
        {
            // load the player data
            MainMenu.instance.playerData = DataHandler.LoadPlayerData();

            coinsText.text = MainMenu.instance.playerData.Coins.ToString();
        }

        public void BoughtItem(string name, int price)
        {

            // remove coins from user
            MainMenu.instance.playerData.Coins -= price;

            // Save Item to db
            DataHandler.SaveIntToDB(name, 1);

            // save player data
            DataHandler.SavePlayerData(MainMenu.instance.playerData);

            // update coins display
            coinsText.text = MainMenu.instance.playerData.Coins.ToString();

        }

    }
}

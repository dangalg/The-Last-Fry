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

        // the player data
        public PlayerData playerData;

        // Use this for initialization
        void Start()
        {

            // load the player data
            playerData = DataHandler.LoadPlayerData();

            ShopPanel.onAfterPurchaseCoinsAction = onAfterPurchaseCoins;

            coinsText.text = playerData.Coins.ToString();

        }

        void onAfterPurchaseCoins(ShopManager.CoinAmount coinAmount)
        {
            // load the player data
            playerData = DataHandler.LoadPlayerData();

            coinsText.text = playerData.Coins.ToString();
        }

        public void BoughtItem(string name, int price)
        {

            // remove coins from user
            playerData.Coins -= price;

            // Save Item to db
            DataHandler.SaveIntToDB(name, 1);

            // save player data
            DataHandler.SavePlayerData(playerData);

            // update coins display
            coinsText.text = playerData.Coins.ToString();

        }

    }
}

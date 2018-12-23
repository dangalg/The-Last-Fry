using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheLastFry
{

    public class ShopManager : MonoBehaviour
    {

        public enum CoinAmount
        {
            HANDFUL,
            PILE,
            SACK,
            BAG,
            CHEST
        }

        public static ShopManager instance = null;

        // The class that handles purchasing from all stores
        Purchaser purchaser = new Purchaser();

        // callbacks
        public UnityAction<CoinAmount> onPurchaseCoinsAction;
        public UnityAction<CoinAmount> onAfterPurchaseCoinsAction;
        public UnityAction onRemoveAds;
        public UnityAction onPurchaseFailedAction;

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

            // initialze shop manager
            Init();
        }

        private void Init()
        {
            purchaser.Initialize();

            purchaser.onRemoveAds = onRemoveAds;
            purchaser.onPurchaseCoins = onPurchaseCoins;
            purchaser.onPurchaseFailed = onPurchaseFailed;
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyHandfulOfCoins()
        {
            purchaser.BuyConsumable(CoinAmount.HANDFUL);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyPileOfCoins()
        {
            purchaser.BuyConsumable(CoinAmount.PILE);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuySackOfCoins()
        {
            purchaser.BuyConsumable(CoinAmount.SACK);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyBagOfCoins()
        {
            purchaser.BuyConsumable(CoinAmount.BAG);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyChestOfCoins()
        {
            purchaser.BuyConsumable(CoinAmount.CHEST);
        }


        /// <summary>
        /// on purchase of gems.
        /// </summary>
        /// <param name="coinAmount">coin amount.</param>
        void onPurchaseCoins(CoinAmount coinAmount)
        {
            if(onPurchaseCoinsAction != null){
                onPurchaseCoinsAction(coinAmount);
            }

            if (onAfterPurchaseCoinsAction != null)
            {
                onAfterPurchaseCoinsAction(coinAmount);
            }

        }

        /// <summary>
        /// on purchase failed.
        /// </summary>
        void onPurchaseFailed()
        {
            // notify player on purchase failed
            if (onPurchaseFailedAction != null)
            {
                onPurchaseFailedAction();
            }
        }

        void onPurchaseRemoveAds(){

            // callback on remove ads and thank player
            if(onRemoveAds != null){
                onRemoveAds();
            }
        }
    }
}

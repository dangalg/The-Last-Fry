using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheLastFry
{

    public class ShopManager : MonoBehaviour
    {

        public enum GemAmount
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
        public UnityAction<GemAmount> onPurchaseGemsAction;
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
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyHandfulOfGems()
        {
            purchaser.BuyConsumable(GemAmount.HANDFUL);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyPileOfGems()
        {
            purchaser.BuyConsumable(GemAmount.PILE);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuySackOfGems()
        {
            purchaser.BuyConsumable(GemAmount.SACK);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyBagOfGems()
        {
            purchaser.BuyConsumable(GemAmount.BAG);
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyChestOfGems()
        {
            purchaser.BuyConsumable(GemAmount.CHEST);
        }


        /// <summary>
        /// on purchase of gems.
        /// </summary>
        /// <param name="gemAmount">Gem amount.</param>
        void onPurchaseGems(GemAmount gemAmount)
        {
            if(onPurchaseGemsAction != null){
                onPurchaseGemsAction(gemAmount);
            }
        }

        /// <summary>
        /// on purchase failed.
        /// </summary>
        void onPurchaseFailed()
        {
            if (onPurchaseFailedAction != null)
            {
                onPurchaseFailedAction();
            }
        }
    }
}

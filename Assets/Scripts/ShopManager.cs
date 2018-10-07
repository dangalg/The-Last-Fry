using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        Purchaser purchaser = new Purchaser();

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

        public void BuyHandfulOfGems()
        {
            purchaser.BuyConsumable(GemAmount.HANDFUL);
        }

        public void BuyPileOfGems()
        {
            purchaser.BuyConsumable(GemAmount.PILE);
        }

        public void BuySackOfGems()
        {
            purchaser.BuyConsumable(GemAmount.SACK);
        }

        public void BuyBagOfGems()
        {
            purchaser.BuyConsumable(GemAmount.BAG);
        }

        public void BuyChestOfGems()
        {
            purchaser.BuyConsumable(GemAmount.CHEST);
        }

        void onPurchaseHandfulOfGems()
        {

        }

        void onPurchasePileOfGems()
        {

        }

        void onPurchaseSackOfGems()
        {

        }

        void onPurchaseBagOfGems()
        {

        }

        void onPurchaseChestOfGems()
        {

        }

        void onPurchaseFailed()
        {

        }
    }
}

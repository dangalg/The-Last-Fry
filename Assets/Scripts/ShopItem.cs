using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheLastFry{

    public class ShopItem : MonoBehaviour
    {

        [SerializeField] string Name;

        [SerializeField] int Price;

        [SerializeField] Button BuyButton;

        [SerializeField] Image Original;
        [SerializeField] Image Shadow;

        [SerializeField] ItemStoreScript ItemStore;

        [SerializeField] GameObject ShopPanel;

        // Use this for initialization
        void Start()
        {

            Shadow.sprite = Resources.Load<Sprite>("Shop\\" + Name + "-shadow");
            Original.sprite = Resources.Load<Sprite>("Shop\\" + Name);

            BuyButton.onClick.AddListener(BuyItem);

            SetupImage();
        }

        void SetupImage()
        {

            // get item owned from db
            bool itemOwned = DataHandler.LoadIntFromDB(Name) == 1;

            if(itemOwned){

                // show image
                Shadow.gameObject.SetActive(false);
                Original.gameObject.SetActive(true);

                // disable buy button
                BuyButton.enabled = false;

            }
            else
            {

                // show Shadow
                Shadow.gameObject.SetActive(true);
                Original.gameObject.SetActive(false);

                // enable buy button
                BuyButton.enabled = true;

            }

        }

        void BuyItem()
        {
            // I have enough coins to buy the item
            if (ItemStore.playerData.Coins >= Price)
            {
                ItemStore.BoughtItem(Name, Price);

                // show image
                Shadow.gameObject.SetActive(false);
                Original.gameObject.SetActive(true);

            }

            // I don't have enough coins
            else
            {
                // show shop panel
                ShopPanel.SetActive(true);
            }
        }
    }
}

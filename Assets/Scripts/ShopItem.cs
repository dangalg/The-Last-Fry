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

        [SerializeField] Text BuyButtonText;

        [SerializeField] Image Original;
        [SerializeField] Image Shadow;

        [SerializeField] ItemStoreScript ItemStore;

        [SerializeField] GameObject ShopPanel;

        [SerializeField] GameObject ItemStorePanel;

        Image buttonImage;

        bool itemOwned = false;

        [SerializeField] bool tutorialButton = false;

        // Use this for initialization
        void Start()
        {

            Shadow.sprite = Resources.Load<Sprite>("Shop\\" + Name + "-shadow");
            Original.sprite = Resources.Load<Sprite>("Shop\\" + Name);

            BuyButton.onClick.AddListener(BuyItem);

            buttonImage = GetComponent<Image>();

            SetupImage();
        }

        void Update()
        {
            if(tutorialButton && buttonImage != null)
            {

                if (DataHandler.LoadIntFromDB(Constants.UNLOCKEDITEM) < 1)
                {
                    buttonImage.color = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(Time.time, 0.5f));
                }

            }
        }


        void SetupImage()
        {

            // get item owned from db
            itemOwned = DataHandler.LoadIntFromDB(Name) == 1;

            if(itemOwned){

                // show image
                Shadow.gameObject.SetActive(false);
                Original.gameObject.SetActive(true);

                // disable buy button
                BuyButton.enabled = false;

                BuyButtonText.text = "Unlocked";

            }
            else
            {

                // show Shadow
                Shadow.gameObject.SetActive(true);
                Original.gameObject.SetActive(false);

                // enable buy button
                BuyButton.enabled = true;

                BuyButtonText.text = Price.ToString();

            }

        }

        void BuyItem()
        {
            if (!itemOwned)
            {
                // I have enough coins to buy the item
                if (MainMenu.instance.playerData.Coins >= Price)
                {
                    ItemStore.BoughtItem(Name, Price);

                    // show image
                    Shadow.gameObject.SetActive(false);
                    Original.gameObject.SetActive(true);

                    BuyButtonText.text = "Unlocked";

                    DataHandler.SaveIntToDB(Constants.UNLOCKEDITEM, 1);

                    itemOwned = true;

                }

                // I don't have enough coins
                else
                {

                    // hide item store
                    ItemStorePanel.SetActive(false);

                    // show shop panel
                    ShopPanel.SetActive(true);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheLastFry
{
    public class StartGameButton : MonoBehaviour
    {


        private Button startButton;

        public UnityAction onFinishedAd;

        private void Start()
        {

            startButton = GetComponent<Button>();



            if (MainMenu.instance.playerData.Energy > 0)
            {

                startButton.GetComponentInChildren<Text>().text = "Start";

            }
            else
            {

                startButton.GetComponentInChildren<Text>().text = "Replenish Energy";

            }

            startButton.onClick.AddListener(LoadGame);
        }

        public void LoadGame()
        {
            if (MainMenu.instance.playerData.Energy > 0)
            {

                MainMenu.instance.DecreaseEnergy(1);
                SceneManager.LoadScene("Game");

            }
            else
            {

                AdManager.instance.PlayAdvertisement();

                MainMenu.instance.playerData.Energy = 3;

                DataHandler.SavePlayerData(MainMenu.instance.playerData);

                if (onFinishedAd != null)
                {
                    onFinishedAd();
                }

                startButton.GetComponentInChildren<Text>().text = "Start";

            }

        }
    }
}

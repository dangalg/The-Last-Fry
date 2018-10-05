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

        private void Start()
        {

            // start button
            startButton = GetComponent<Button>();


            // if out of energy display replenish otherwise display start
            if (MainMenu.instance.playerData.Energy > 0)
            {
                // start game
                startButton.GetComponentInChildren<Text>().text = "Start";

            }
            else
            {
                // display 
                startButton.GetComponentInChildren<Text>().text = "Replenish Energy";

            }

        }


    }
}

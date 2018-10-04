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

            startButton = GetComponent<Button>();



            if (MainMenu.instance.playerData.Energy > 0)
            {

                startButton.GetComponentInChildren<Text>().text = "Start";

            }
            else
            {

                startButton.GetComponentInChildren<Text>().text = "Replenish Energy";

            }

        }


    }
}

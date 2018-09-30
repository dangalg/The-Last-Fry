using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResetLevelsButton : MonoBehaviour {

    public Button resetButton;

    public UnityAction onFinishedReset;

    private void Start()
    {
        resetButton = GetComponent<Button>();

        resetButton.onClick.AddListener(ResetLevels);
    }

    public void ResetLevels()
    {
        MainMenu.instance.playerData = new PlayerData();

        DataHandler.SavePlayerData(MainMenu.instance.playerData);

        if(onFinishedReset != null){
            onFinishedReset();
        }

    }
}

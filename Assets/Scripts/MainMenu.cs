using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance = null;

    public PlayerData playerData;

    [SerializeField] ResetLevelsButton resetButton;
    [SerializeField] TMP_Text Record;
    [SerializeField] TMP_Text Energy;

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

        resetButton.onFinishedReset = onResetClicked;

        //Call the InitGame function to initialize the first level 
        InitMenu();
    }

    void InitMenu(){
        playerData = DataHandler.LoadPlayerData();
    }

    void onResetClicked()
    {
        Record.text = playerData.Record.ToString();
        Energy.text = playerData.Energy.ToString();
    }

    // Use this for initialization
    void Start () {
        Record.text = playerData.Record.ToString();
        Energy.text = playerData.Energy.ToString();
	}

    public void DecreaseEnergy(int amount){
        playerData.Energy -= amount;
        savePlayerData();
    }

    public void IncreaseEnergy(int amount){
        playerData.Energy += amount;
        savePlayerData();
    }

    void savePlayerData(){
        DataHandler.SavePlayerData(playerData);
    }
	
}

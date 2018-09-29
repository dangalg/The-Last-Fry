using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {


    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
//    private int level = 3;                                  //Current level number, expressed in game as "Day 1".
    private int points = 0;                                  //Current points owned by player
    [SerializeField] TMP_Text pointsText;
    public PlayerData playerData;


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
        DontDestroyOnLoad(gameObject);

        playerData = new PlayerData();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {


    }

    public void AddPoint(int pointsToAdd)
    {
        points += pointsToAdd;
        displayPoints();
    }

    public void SubtractPoints(int pointsToAdd)
    {
        points -= pointsToAdd;
        displayPoints();
    }

    private void displayPoints()
    {
        pointsText.text = points.ToString();
    }

    public void EndGame(){

    }

}

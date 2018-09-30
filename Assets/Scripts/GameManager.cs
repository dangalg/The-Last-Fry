﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
//    private int level = 3;                                  //Current level number, expressed in game as "Day 1".
    private int points = 0;                                  //Current points owned by player
    [SerializeField] TMP_Text pointsText;
    [SerializeField] TMP_Text levelText;

    PlayerData playerData;


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

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        playerData = DataHandler.LoadPlayerData();

        points = 0;
        displayPoints();

        StartCoroutine(SetupGame(playerData.Level));

    }

    IEnumerator SetupGame(int level){

        levelText.enabled = true;

        levelText.text = level.ToString() + " Hand";

        yield return new WaitForSeconds(5f);

        levelText.enabled = false;

        FrySpawner.instance.Reset();
        HandSpawner.instance.Reset();

        FrySpawner.instance.Setup(playerData.Level);

        yield return new WaitForSeconds(1f);

        HandSpawner.instance.Setup(playerData.Level);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HitHand(Input.mousePosition);
        }
#else
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            ClickHand(Input.GetTouch(0).position);
        }
#endif
    }

    private void HitHand(Vector3 hitPosition)
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(hitPosition), Vector2.zero);

        //Converting Mouse Pos to 2D (vector2) World Pos
        if (hit.collider != null)
        {
            Debug.Log("Something Hit");

            if (hit.collider.CompareTag("ThiefHand"))
            {
                hit.collider.gameObject.GetComponent<HandController>().HitHand();

            }
        }
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
        SceneManager.LoadScene("MainMenu");
    }

    public void EndLevel(){

        Debug.Log("EndLevel");

        playerData.Level++;

        if (playerData.Record < points)
        {
            playerData.Record = points;
        }

        DataHandler.SavePlayerData(playerData);

        StartCoroutine(SetupGame(playerData.Level));
    }

}

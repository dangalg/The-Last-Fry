using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandSpawner : MonoBehaviour {

    public static HandSpawner instance = null;

    public int handHitCounter = 0;
    public int handGotFryCounter = 0;

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


    [SerializeField] GameObject HandsHolder;
    [SerializeField] GameObject hand;

    public List<GameObject> Hands;

    public float distanceX = 5f;
    public float distanceY = 7f;
    public float minHandStartSpeed = 3f;
    public float maxHandStartSpeed = 7f;
    public float minTimeBetweenSpawns = 2;
    public float maxTimeBetweenSpawns = 5;
    public int handAmount = 3;

    public void Setup(int level){

        handAmount = level;

        StartCoroutine(SpawnHands());
    }

    public void Reset()
    {
        Hands.Clear();

        handGotFryCounter = 0;
        handHitCounter = 0;

        foreach (Transform child in HandsHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void onHandGotFry()
    {
        handGotFryCounter++;
        CheckEndGame();
    }


    void onHandHit()
    {
        handHitCounter++;
        CheckEndGame();
    }

    void CheckEndGame(){
        if(handGotFryCounter + handHitCounter >= handAmount){
            GameManager.instance.EndLevel();
        }
    }


    private Vector3 SetupSpawnPoint()
    {
        float randomXDistance = Random.Range(0, distanceX);
        float randomYDistance = Random.Range(0, distanceY);

        int randomXOrY = Random.Range(0, 2);
        int randomMinus = Random.Range(0, 2);

        bool minus = randomMinus == 1;
        bool xOrY = randomXOrY == 1;

        Vector3 returnPosition;

        if(minus){
            randomXDistance *= -1;
            randomYDistance *= -1;
        }

        if (xOrY)
        {
            returnPosition = new Vector3(distanceX, randomYDistance, 0);
        }
        else
        {
            returnPosition = new Vector3(randomXDistance, distanceY, 0);
        }

        return returnPosition;

    }

    private void SpwanHand()
    {
        int randomFreeFryIndex = FrySpawner.instance.GetRandomFreeFryIndex();

        if (randomFreeFryIndex != -1)
        {
            Vector3 spawnPoint = SetupSpawnPoint();

            GameObject handObject = Instantiate(hand, HandsHolder.transform);
            handObject.transform.position = spawnPoint;

            HandController handController = handObject.GetComponent<HandController>();
            float randomHandSpeed = Random.Range(minHandStartSpeed, maxHandStartSpeed);
            handController.moveSpeed = randomHandSpeed;

            handController.targetFry = FrySpawner.instance.Fries[randomFreeFryIndex];
            handController.fryIndex = randomFreeFryIndex;
            handController.handGotFry = onHandGotFry;
            handController.handGotHit = onHandHit;

            Hands.Add(handObject);
        }

    }

    IEnumerator SpawnHands(){

        for (int i = 0; i < handAmount; i++)
        {
            SpwanHand();

            float randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

            yield return new WaitForSeconds(randomSpawnTime);
        }
    }

}

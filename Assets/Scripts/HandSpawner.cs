using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpawner : MonoBehaviour {

    public static HandSpawner instance = null;

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
    }

    [SerializeField] GameObject HandsHolder;
    [SerializeField] GameObject hand;

    public List<GameObject> Hands;

    public float distance = 5f;
    public float minHandStartSpeed = 3f;
    public float maxHandStartSpeed = 7f;
    public int minTimeBetweenSpawns = 2;
    public int maxTimeBetweenSpawns = 5;
    public int numberOfHandsToSpawn = 3;

    bool xOrY = true;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnHands());
    }

    private Vector3 SetupSpawnPoint()
    {
        float randomDistance = Random.Range(-distance, distance);

        int randomXOrY = Random.Range(0, 1);

        xOrY = randomXOrY == 1;

        Vector3 returnPosition;

        if (xOrY)
        {
            returnPosition = new Vector3(distance, randomDistance, 0);
        }
        else
        {
            returnPosition = new Vector3(randomDistance, distance, 0);
        }

        return returnPosition;

    }

    private void SpwanHand()
    {


        Vector3 spawnPoint = SetupSpawnPoint();

        GameObject handObject = Instantiate(hand, HandsHolder.transform);
        handObject.transform.position = spawnPoint;

        HandController handController = handObject.GetComponent<HandController>();
        float randomHandSpeed = Random.Range(minHandStartSpeed, maxHandStartSpeed);
        handController.moveSpeed = randomHandSpeed;
        int randomFreeFryIndex = FrySpawner.instance.GetRandomFreeFryIndex();
        handController.targetFry = FrySpawner.instance.Fries[randomFreeFryIndex];
        handController.fryIndex = randomFreeFryIndex;

        Hands.Add(handObject);
    }

    IEnumerator SpawnHands(){

        yield return new WaitForSeconds(1);

        for (int i = 0; i < numberOfHandsToSpawn; i++)
        {
            SpwanHand();

            int randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

            yield return new WaitForSeconds(randomSpawnTime);
        }
    }
	
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrySpawner : MonoBehaviour {

    public static FrySpawner instance = null;

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

    [SerializeField] GameObject FriesHolder;
    public List<GameObject> Fries;

    [SerializeField] GameObject fry;

    public float distance = 1.7f;
    public int fryAmount = 10;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpawnFries());
    }

    private Vector3 SetupSpawnPoint()
    {
        float randomXDistance = Random.Range(-distance, distance);
        float randomYDistance = Random.Range(-distance, distance);

        Vector3 returnPosition;
        returnPosition = new Vector3(randomXDistance, randomYDistance, 0);

        return returnPosition;

    }

    private void SpwanFry()
    {

        int randomfryRotation = Random.Range(0, 360);
        Quaternion randomRotation = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, randomfryRotation)
        };

        Vector3 frySpawnPosition = SetupSpawnPoint();

        GameObject fryObject = Instantiate(fry, FriesHolder.transform);

        fryObject.transform.position = frySpawnPosition;
        fryObject.transform.rotation = randomRotation;

        Fries.Add(fryObject);
    }

    IEnumerator SpawnFries()
    {
        for (int i = 0; i < fryAmount; i++)
        {
            SpwanFry();

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DestroyFry(int index){
        Destroy(Fries[index]);
        Fries.RemoveAt(index);
    }
}

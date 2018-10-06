﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{
    public class ThiefSpawner : Spawner
    {

        public static ThiefSpawner instance = null;

        // counter for thieves hit
        public int thiefHitCounter = 0;

        // counter for food stolen
        public int thiefGotFoodCounter = 0;

        // the multiplier that makes level and spawn time faster for every level
        float levelMultiplier = 1f;

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

        /// <summary>
        /// Reset Spawner to beginning
        /// </summary>
        public override void Reset()
        {
            // clear items
            itemAmount = 0;
            Items.Clear();

            // clear counters
            thiefGotFoodCounter = 0;
            thiefHitCounter = 0;

            // destroy Items
            foreach (Transform child in itemHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Called when a thief steals food
        /// </summary>
        void onThiefStoleFood()
        {
            // increment thief got food counter
            thiefGotFoodCounter++;

            // check to see if level has ended
            CheckEndGame();
        }

        /// <summary>
        /// Called when a thief gets hit
        /// </summary>
        void onThiefHit()
        {

            // increment thief got hit counter
            thiefHitCounter++;

            // check to see if level has ended
            CheckEndGame();
        }

        /// <summary>
        /// Check to see if game has ended
        /// </summary>
        void CheckEndGame()
        {

            // all thieves for level have either stolen successfully or have been hit
            bool noMoreThieves = thiefGotFoodCounter + thiefHitCounter >= itemAmount;

            // level has been reset if there are no items in the list
            bool levelHasNotBeenReset = itemAmount > 0;

            // if the level has not been reset and there are no more thieves 
            if (levelHasNotBeenReset && noMoreThieves)
            {
                // move to next level
                StartCoroutine(GameManager.instance.NextLevel());
            }
        }

        /// <summary>
        /// Create many thieves
        /// </summary>
        /// <returns>The items.</returns>
        protected override IEnumerator SpawnItems()
        {

            // set a level multiplier to make hands faster for each level
            levelMultiplier = 1.0f - (((float)level) * 0.001f);

            for (int i = 0; i < itemAmount; i++)
            {
                // create one thief
                SpwanThief();

                // set a random spawn time until next thief
                float randomSpawnTime = Random.Range(minTimeBetweenSpawns * levelMultiplier, maxTimeBetweenSpawns * levelMultiplier);

                yield return new WaitForSeconds(randomSpawnTime);
            }
        }

        /// <summary>
        /// Create one thief and sets it's target food
        /// </summary>
        private void SpwanThief()
        {
            // get a random free food as the target for theft
            int randomFreeFoodIndex = FoodSpawner.instance.GetRandomFreeFoodIndex();

            // if a random food has been returned
            if (randomFreeFoodIndex != -1)
            {

                // get food target position and set y positive or negative based on its position
                // this is so that thief will come into screen from the closest direction to the food

                // set a random spawn point
                Vector3 spawnPoint = SetupSpawnPoint(true);

                // Create thief
                GameObject thiefObject = Instantiate(GetRandomItemTypeIndex(), itemHolder.transform);

                // set thief properties
                ThiefController thiefController = thiefObject.GetComponent<ThiefController>();

                // set begin position
                thiefController.beginPosition = spawnPoint;

                // set end position
                thiefController.endPosition = spawnPoint;

                // set random speed
                float randomHandSpeed = Random.Range(minSpeed * levelMultiplier, maxSpeed * levelMultiplier);
                thiefController.moveSpeed = randomHandSpeed;

                // set target food to steal
                thiefController.targetFood = FoodSpawner.instance.Items[randomFreeFoodIndex];

                // set food index to steal
                thiefController.foodIndex = randomFreeFoodIndex;

                // set the function to call on thief stole food successfully
                thiefController.thiefGotFood += onThiefStoleFood;

                // set the function to call on thief got hit
                thiefController.thiefGotHit += onThiefHit;

                // setupThief
                thiefController.Setup();

                // add thief to thieves list
                Items.Add(thiefObject);
            }

        }
    }
}

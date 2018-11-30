using System.Collections;
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

        // coin spawn speed
        [SerializeField] float coinSpawnSpeed = 0.1f;

        // list of all itemTypes
        public List<GameObject> allItemTypes;

        // list of all item type odds
        public List<int> allItemTypesOdds;

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

            allItemTypes = new List<GameObject>(itemTypes);

            allItemTypesOdds = new List<int>(itemTypesOdds);

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
                GameManager.instance.GoToNextLevel();
            }
        }


        /// <summary>
        /// Setup the item types for current level.
        /// </summary>
        void setupItemTypesForCurrentLevel(int gameLevel)
        {

            // get all item types intolist
            itemTypes = new List<GameObject>(allItemTypes);

            // get all item type odds into list
            itemTypesOdds = new List<int>(allItemTypesOdds);


            // iterate through item types
            for (int i = itemTypes.Count - 1; i > 0; i--)
            {

                // get thief controller from current object
                ThiefController thiefController = itemTypes[i].gameObject.GetComponent<ThiefController>();

                // remove any item types not fitting the level.
                //if (thiefController.ThiefLevel > gameLevel)
                //{
                //    itemTypes.RemoveAt(i);
                //    itemTypesOdds.RemoveAt(i);

                //    continue;
                //}

                // get item owned from db
                bool itemOwned = DataHandler.LoadIntFromDB(thiefController.ThiefName) == 1;

                // remove any item types not bought.
                if (!itemOwned)
                {
                    itemTypes.RemoveAt(i);
                    itemTypesOdds.RemoveAt(i);

                    continue;
                }

            }
        }

        /// <summary>
        /// Create many thieves
        /// </summary>
        /// <returns>The items.</returns>
        protected override IEnumerator SpawnItems(int gameLevel)
        {

            // setup the item types for the current level
            setupItemTypesForCurrentLevel(gameLevel);

            // set a level multiplier to make hands faster for each level
            levelMultiplier = 1.0f - (((float)level) * 0.003f);

            for (int i = 0; i < itemAmount; i++)
            {
                // create one thief
                SpwanThief(gameLevel);

                // set a random spawn time until next thief
                float randomSpawnTime = Random.Range(minTimeBetweenSpawns * levelMultiplier, maxTimeBetweenSpawns * levelMultiplier);

                yield return new WaitForSeconds(randomSpawnTime);
            }
        }

        /// <summary>
        /// Create one thief and sets it's target food
        /// </summary>
        /// <param name="gameLevel">Game level.</param>
        private void SpwanThief(int gameLevel)
        {
            // get a random free food as the target for theft
            int randomFreeFoodIndex = FoodSpawner.instance.GetRandomFreeFoodIndex();

            // if a random food has been returned
            if (randomFreeFoodIndex != -1)
            {

                // get food target position and set y positive or negative based on its position
                // this is so that thief will come into screen from the closest direction to the food

                // get random target for thief
                GameObject targetFood = FoodSpawner.instance.Items[randomFreeFoodIndex];

                bool targetYIsPositive = targetFood.transform.position.y > 0; 

                // set a random spawn point
                Vector3 spawnPoint = SetupSpawnPoint(true, targetYIsPositive);

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
                thiefController.targetFood = targetFood;

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

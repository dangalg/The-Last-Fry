using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public class UntouchableSpawner : Spawner
    {

        public static UntouchableSpawner instance = null;



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

        public override void Reset()
        {
        }

        /// <summary>
        /// Spawns the items.
        /// </summary>
        /// <returns>The items.</returns>
        protected override IEnumerator SpawnItems()
        {
            // spawn all untouchables one by one at spawn time
            for (int i = 0; i < itemAmount; i++)
            {
                // spawn untouchable
                SpwanUntouchable();

                // get random spawn time
                float randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

                yield return new WaitForSeconds(randomSpawnTime);
            }
        }

        /// <summary>
        /// Create one thief and sets it's target food
        /// </summary>
        private void SpwanUntouchable()
        {
            // get a random free food as the target for theft
            int randomFreeFoodIndex = FoodSpawner.instance.GetRandomFreeFoodIndex();

            // if a random food has been returned
            if (randomFreeFoodIndex != -1)
            {
                // set a random spawn point
                Vector3 spawnPoint = SetupSpawnPoint(true);

                // Create thief
                GameObject untouchableObject = Instantiate(GetRandomItemTypeIndex(), itemHolder.transform);

                // set his spawn point
                untouchableObject.transform.position = spawnPoint;

                // set thief properties
                UntouchableController untouchableontroller = untouchableObject.GetComponent<UntouchableController>();

                // set random speed
                float randomHandSpeed = Random.Range(minSpeed, maxSpeed);
                //untouchableontroller.moveSpeed = randomHandSpeed;

                // set target food to steal
                //untouchableontroller.targetFood = FoodSpawner.instance.Items[randomFreeFoodIndex];

                // set food index to steal
                //untouchableontroller.foodIndex = randomFreeFoodIndex;

                // add thief to thieves list
                Items.Add(untouchableObject);
            }

        }


    }
}

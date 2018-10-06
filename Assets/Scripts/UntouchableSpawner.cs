using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public class UntouchableSpawner : Spawner
    {

        public static UntouchableSpawner instance = null;

        // reset check
        bool reset = false;

        // The index to know what untouchable has already been spawned
        int untouchableSpawnedIndex = 0;

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
            reset = true;

            // clear items
            itemAmount = 0;
            Items.Clear();

            // clear counters
            untouchableSpawnedIndex = 0;

            // destroy Items
            foreach (Transform child in itemHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Spawns the items.
        /// </summary>
        /// <returns>The items.</returns>
        protected override IEnumerator SpawnItems()
        {
            reset = false;
            // spawn all untouchables one by one at spawn time
            // in untouchables We spawn items by the item amount specified 
            // if they are destroyed we spawn the next item amount
            for (int i = 0; i < itemTypes.Count; i++)
            {
                if (!reset)
                {
                    // spawn untouchable
                    SpwanUntouchable();

                    // get random spawn time
                    float randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

                    yield return new WaitForSeconds(randomSpawnTime);
                }
            }
        }

        /// <summary>
        /// Create one thief and sets it's target food
        /// </summary>
        private void SpwanUntouchable()
        {

            // set a random spawn point
            Vector3 spawnPoint = SetupSpawnPoint(true);

            // Create thief
            GameObject untouchableObject = Instantiate(itemTypes[untouchableSpawnedIndex], itemHolder.transform);

            // up the index to get the next untouchable next spawn time
            untouchableSpawnedIndex++;

            // set his spawn point
            untouchableObject.transform.position = spawnPoint;

            // set Item properties
            UntouchableController untouchableontroller = untouchableObject.GetComponent<UntouchableController>();

            // set random speed
            float randomHandSpeed = Random.Range(minSpeed, maxSpeed);
            untouchableontroller.moveSpeed = randomHandSpeed;

            // set callback for item
            untouchableontroller.onCompleteFloat = onUntouchableCompleteFlight;

            // set callback for item hit
            untouchableontroller.onHitUntouchable = onHitUntouchable;

            // start floating untouchable
            untouchableontroller.floatToTraget(SetupSpawnPoint(true));

            // add thief to thieves list
            Items.Add(untouchableObject);

            // store index for future reference
            untouchableontroller.indexOfUntouchableInList = Items.Count - 1;

        }

        /// <summary>
        /// untouchable complete flight.
        /// </summary>
        /// <param name="indexOfUntouchableInList">Index of untouchable in list.</param>
        void onUntouchableCompleteFlight(int indexOfUntouchableInList){

            // get contoroller of item
            UntouchableController untouchableontroller = Items[indexOfUntouchableInList].GetComponent<UntouchableController>();

            // float to target
            untouchableontroller.floatToTraget(SetupSpawnPoint(false));

        }

        /// <summary>
        /// hit untouchable.
        /// </summary>
        /// <param name="amountOfLifeToLose">Amount of life to lose.</param>
        void onHitUntouchable(int amountOfLifeToLose){

            // lose life when hitting untouchable
            GameManager.instance.LoseLife(amountOfLifeToLose);

        }


    }
}

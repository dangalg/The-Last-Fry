using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public class OneUpSpawner : Spawner
    {

        public static OneUpSpawner instance = null;

        // reset check
        bool reset = false;

        // The index to know what untouchable has already been spawned
        int oneUpSpawnedIndex = 0;

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
            oneUpSpawnedIndex = 0;

            // destroy Items
            foreach (Transform child in itemHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

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
                    SpwanOneUp();

                    // get random spawn time
                    float randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

                    yield return new WaitForSeconds(randomSpawnTime);
                }
            }
        }

        /// <summary>
        /// Create one thief and sets it's target food
        /// </summary>
        private void SpwanOneUp()
        {

            // set a random spawn point
            Vector3 spawnPoint = SetupSpawnPoint(true);

            // Create thief
            GameObject oneUpObject = Instantiate(itemTypes[oneUpSpawnedIndex], itemHolder.transform);

            // up the index to get the next untouchable next spawn time
            oneUpSpawnedIndex++;

            // set his spawn point
            oneUpObject.transform.position = spawnPoint;

            // set Item properties
            OneUpController oneUpController = oneUpObject.GetComponent<OneUpController>();

            // set random speed
            float randomHandSpeed = Random.Range(minSpeed, maxSpeed);
            oneUpController.moveSpeed = randomHandSpeed;

            // set callback for item
            oneUpController.onCompleteFloat = onOneUpCompleteFlight;

            // set callback for item hit
            oneUpController.onHitOneUp = onHitOneUp;

            // start floating untouchable
            oneUpController.floatToTraget(SetupSpawnPoint(true));

            // add thief to thieves list
            Items.Add(oneUpObject);

            // store index for future reference
            oneUpController.indexOfOneUpInList = Items.Count - 1;

        }


        /// <summary>
        /// untouchable complete flight.
        /// </summary>
        /// <param name="indexOfOneUpInList">Index of untouchable in list.</param>
        void onOneUpCompleteFlight(int indexOfOneUpInList)
        {

            // get contoroller of item
            OneUpController oneUpontroller = Items[indexOfOneUpInList].GetComponent<OneUpController>();

            // float to target
            oneUpontroller.floatToTraget(SetupSpawnPoint(false));

        }

        /// <summary>
        /// hit untouchable.
        /// </summary>
        /// <param name="amountOfLifeToGain">Amount of life to lose.</param>
        void onHitOneUp(int amountOfLifeToGain)
        {
            // lose life when hitting untouchable
            GameManager.instance.GainLife(amountOfLifeToGain);

        }
    }
}

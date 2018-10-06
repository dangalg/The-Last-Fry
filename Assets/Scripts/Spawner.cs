using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public abstract class Spawner : MonoBehaviour
    {

        // distance to set spawn point at
        public float distanceX = 5f;
        public float distanceY = 7f;

        // speed to move
        public float minSpeed = 3f;
        public float maxSpeed = 7f;

        // time between spawns
        public float minTimeBetweenSpawns = 2;
        public float maxTimeBetweenSpawns = 5;

        // amount to create
        protected int itemAmount = 0;

        // the list of items
        public List<GameObject> Items;

        // the game object holding the items
        [SerializeField] protected GameObject itemHolder;

        // odds for getting an item type
        public List<int> itemTypesOdds;

        // the prefabs for item types to instantiate objects from
        public List<GameObject> itemTypes;

        // the level the game is on
        protected int level = 1;

        /// <summary>
        /// Setup the specified level.
        /// </summary>
        /// <param name="level">Level.</param>
        public void Setup(int gameLevel)
        {
            // the amount of hands is based on the level TODO think how to change this it is stupid
            itemAmount = gameLevel;
            level = gameLevel;

            // start game
            StartCoroutine(SpawnItems());
        }

        // Update is called once per frame
        public abstract void Reset();

        /// <summary>
        /// Gets the random index of the item type.
        /// </summary>
        /// <returns>The random item type index.</returns>
        protected GameObject GetRandomItemTypeIndex()
        {

            // the max odd number
            int maxNumber = itemTypesOdds[itemTypesOdds.Count - 1];

            // bet a random number between zero and the max odd
            int randomItemType = Random.Range(0, maxNumber);

            // get the random object selected
            for (int i = 0; i < itemTypesOdds.Count; i++)
            {
                // when reached the random number that is the selected object
                if (itemTypesOdds[i] > randomItemType)
                {
                    return itemTypes[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Setups the spawn point.
        /// </summary>
        /// <returns>The spawn point.</returns>
        /// <param name="onlyOnEdges">If set to <c>true</c> only on edges.</param>
        /// <param name="overrideYMinusOrPlus">If this is true y will always be positive, if false y will be negative.</param>
        protected Vector3 SetupSpawnPoint(bool onlyOnEdges, bool? overrideYMinusOrPlus = null)
        {
            // get random distance
            float randomXDistance = Random.Range(0, distanceX);
            float randomYDistance = Random.Range(0, distanceY);

            // distance calculated will be minus or plus based on the random
            float distanceXCaculated = distanceX;
            float distanceYCaculated = distanceY;

            // randomize x or y this is for always starting on the edge
            int randomXOrY = Random.Range(0, 2);
            bool xOrY = randomXOrY == 1;

            // should x or y be minus
            int randomXMinus = Random.Range(0, 2);
            int randomYMinus = Random.Range(0, 2);

            // convert ints to bool for no apparent reason can be in one line..
            bool minusX = randomXMinus == 1;
            bool minusY = randomYMinus == 1;

            // get random x and y distance
            int randomDistanceXMinus = Random.Range(0, 2);
            int randomDistanceYMinus = Random.Range(0, 2);

            // convert ints to bool for no apparent reason can be in one line.. 
            bool minusDistanceX = randomDistanceXMinus == 1;
            bool minusDistanceY = randomDistanceYMinus == 1;

            // override xOrY by user
            if (overrideYMinusOrPlus != null)
            {
                if((bool)overrideYMinusOrPlus)
                {
                    minusDistanceY = false;
                    minusY = false;
                }
                else
                {
                    minusDistanceY = true;
                    minusY = true;
                }
            }

            // the random position to return
            Vector3 returnPosition;

            // set minus x
            if (minusX)
            {
                randomXDistance *= -1;
            }

            // set minus y
            if (minusY)
            {
                randomYDistance *= -1;
            }

            // set minus x for edges
            if (minusDistanceX)
            {
                distanceXCaculated *= -1;
            }

            // set minus x for edges
            if (minusDistanceY)
            {
                distanceYCaculated *= -1;
            }

            // only on edges either x or y have to be max distance number
            if(onlyOnEdges){

                // either x or y are on the edge
                if (xOrY)
                {
                    // x is on the edge
                    returnPosition = new Vector3(distanceXCaculated, randomYDistance, 0);
                }
                else
                {
                    // y is on the edge
                    returnPosition = new Vector3(randomXDistance, distanceYCaculated, 0);
                }
            }
            else{
                // randomize everything can be anywhere inside the limits
                returnPosition = new Vector3(randomXDistance, randomYDistance, 0);
            }

            // return position
            return returnPosition;

        }

        // a spawner must spawn!
        protected abstract IEnumerator SpawnItems();
    }
}

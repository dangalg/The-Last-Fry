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

        // 
        public List<GameObject> Items;
        [SerializeField] protected GameObject itemHolder;
        public List<int> itemTypesOdds;
        public List<GameObject> itemTypes;

        public void Setup(int level)
        {
            itemAmount = level;

            StartCoroutine(SpawnItems());
        }

        // Update is called once per frame
        public abstract void Reset();

        protected GameObject GetRandomItemTypeIndex()
        {

            int maxNumber = itemTypesOdds[itemTypesOdds.Count - 1];

            int randomItemType = Random.Range(0, maxNumber);

            for (int i = 0; i < itemTypesOdds.Count; i++)
            {
                if (itemTypesOdds[i] > randomItemType)
                {
                    return itemTypes[i];
                }
            }

            return null;
        }

        protected Vector3 SetupSpawnPoint(bool onlyOnEdges)
        {
            float randomXDistance = Random.Range(0, distanceX);
            float randomYDistance = Random.Range(0, distanceY);

            float distanceXCaculated = distanceX;
            float distanceYCaculated = distanceY;

            int randomXOrY = Random.Range(0, 2);
            bool xOrY = randomXOrY == 1;

            int randomXMinus = Random.Range(0, 2);
            int randomYMinus = Random.Range(0, 2);

            bool minusX = randomXMinus == 1;
            bool minusY = randomYMinus == 1;

            int randomDistanceXMinus = Random.Range(0, 2);
            int randomDistanceYMinus = Random.Range(0, 2);

            bool minusDistanceX = randomDistanceXMinus == 1;
            bool minusDistanceY = randomDistanceYMinus == 1;


            Vector3 returnPosition;

            if (minusX)
            {
                randomXDistance *= -1;
            }

            if (minusY)
            {
                randomYDistance *= -1;
            }

            if (minusDistanceX)
            {
                distanceXCaculated *= -1;
            }

            if (minusDistanceY)
            {
                distanceYCaculated *= -1;
            }

            if(onlyOnEdges){
                if (xOrY)
                {
                    returnPosition = new Vector3(distanceXCaculated, randomYDistance, 0);
                }
                else
                {
                    returnPosition = new Vector3(randomXDistance, distanceYCaculated, 0);
                }
            }
            else{
                returnPosition = new Vector3(randomXDistance, randomYDistance, 0);
            }



            return returnPosition;

        }

        protected abstract IEnumerator SpawnItems();
    }
}

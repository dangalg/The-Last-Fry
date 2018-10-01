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

        protected override IEnumerator SpawnItems()
        {

            for (int i = 0; i < itemAmount; i++)
            {
                SpwanUntouchable();

                float randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

                yield return new WaitForSeconds(randomSpawnTime);
            }
        }

        private void SpwanUntouchable()
        {
            int randomFreeFryIndex = FrySpawner.instance.GetRandomFreeFryIndex();

            if (randomFreeFryIndex != -1)
            {
                Vector3 spawnPoint = SetupSpawnPoint(true);

                GameObject itemObject = Instantiate(GetRandomItemTypeIndex(), itemHolder.transform);
                itemObject.transform.position = spawnPoint;

                Items.Add(itemObject);
            }

        }


    }
}

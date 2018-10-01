using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public class FrySpawner : Spawner
    {

        public static FrySpawner instance = null;

        public List<int> TakenFryIndexes = new List<int>();

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

        protected override IEnumerator SpawnItems()
        {

            yield return null;

            for (int i = 0; i < itemAmount; i++)
            {
                SpwanFry();
            }
        }

        public override void Reset()
        {
            Items.Clear();
            TakenFryIndexes.Clear();

            foreach (Transform child in itemHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void SpwanFry()
        {

            int randomfryRotation = Random.Range(0, 360);
            Quaternion randomRotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, randomfryRotation)
            };

            Vector3 frySpawnPosition = SetupSpawnPoint(false);

            GameObject fryObject = Instantiate(GetRandomItemTypeIndex(), itemHolder.transform);

            fryObject.transform.position = frySpawnPosition;
            fryObject.transform.rotation = randomRotation;

            Items.Add(fryObject);
        }

        public void DestroyFry(int index)
        {
            Destroy(Items[index]);
        }

        public int GetRandomFreeFryIndex()
        {
            List<int> freeFryIndexes = new List<int>();

            for (int i = 0; i < Items.Count; i++)
            {
                freeFryIndexes.Add(i);
            }

            foreach (var item in TakenFryIndexes)
            {
                freeFryIndexes.Remove(item);
            }

            int randomfryToTake = Random.Range(0, freeFryIndexes.Count);
            int freeFryIndex = -1;

            freeFryIndex = freeFryIndexes[randomfryToTake];

            TakenFryIndexes.Add(freeFryIndex);

            return freeFryIndex;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{
    public class HandSpawner : Spawner
    {

        public static HandSpawner instance = null;

        public int handHitCounter = 0;
        public int handGotFryCounter = 0;

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
            itemAmount = 0;
            Items.Clear();

            handGotFryCounter = 0;
            handHitCounter = 0;

            foreach (Transform child in itemHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        void onHandGotFry()
        {
            handGotFryCounter++;
            CheckEndGame();
        }


        void onHandHit()
        {
            handHitCounter++;
            CheckEndGame();
        }

        void CheckEndGame()
        {
            if (handGotFryCounter + handHitCounter >= itemAmount)
            {
                GameManager.instance.NextLevel();
            }
        }

        protected override IEnumerator SpawnItems()
        {

            for (int i = 0; i < itemAmount; i++)
            {
                SpwanHand();

                float randomSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

                yield return new WaitForSeconds(randomSpawnTime);
            }
        }

        private void SpwanHand()
        {
            int randomFreeFryIndex = FrySpawner.instance.GetRandomFreeFryIndex();

            if (randomFreeFryIndex != -1)
            {
                Vector3 spawnPoint = SetupSpawnPoint(true);

                GameObject handObject = Instantiate(GetRandomItemTypeIndex(), itemHolder.transform);
                handObject.transform.position = spawnPoint;

                HandController handController = handObject.GetComponent<HandController>();
                float randomHandSpeed = Random.Range(minSpeed, maxSpeed);
                handController.moveSpeed = randomHandSpeed;

                handController.targetFry = FrySpawner.instance.Items[randomFreeFryIndex];
                handController.fryIndex = randomFreeFryIndex;
                handController.handGotFry += onHandGotFry;
                handController.handGotHit += onHandHit;

                Items.Add(handObject);
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public class FoodSpawner : Spawner
    {

        public static FoodSpawner instance = null;

        public List<int> TakenFoodIndexes = new List<int>();

        [SerializeField] GameObject CoinCounterTarget;
        [SerializeField] GameObject Coin;
        [SerializeField] float coinMoveSpeed = 2f;
        [SerializeField] LeanTweenType coinFlyEaseType = LeanTweenType.easeInOutCubic;

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

        // Generate Food
        protected override IEnumerator SpawnItems()
        {

            yield return null;

            // Make food
            for (int i = 0; i < itemAmount; i++)
            {
                SpwanItem();
            }
        }

        // Reset Spawner to beginning
        public override void Reset()
        {
            // clear items
            Items.Clear();
            TakenFoodIndexes.Clear();

            // destroy Items
            foreach (Transform child in itemHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Spawn one Item
        private void SpwanItem()
        {
            // get random rotation
            int randomfryRotation = Random.Range(0, 360);
            Quaternion randomRotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, randomfryRotation)
            };

            // get random position
            Vector3 frySpawnPosition = SetupSpawnPoint(false);

            // create item on screen
            GameObject fryObject = Instantiate(GetRandomItemTypeIndex(), itemHolder.transform);

            // set items position
            fryObject.transform.position = frySpawnPosition;
            fryObject.transform.rotation = randomRotation;

            // get the controller script of the item
            FoodController fryController = fryObject.GetComponent<FoodController>();

            // add item to list
            Items.Add(fryObject);
        }

        // Go to next level
        public IEnumerator NextLevelRoutine(){

            // for every food item
            foreach (var item in FoodSpawner.instance.Items)
            {

                if (item != null)
                {
                    // create a coin in the food item position and rotation and set it's properties
                    createCoin(item);

                    // destroy food item
                    Destroy(item);
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        // Create a coin
        private void createCoin(GameObject item)
        {
            // create the object
            GameObject coinObject = Instantiate(Coin, item.transform.position, item.transform.rotation);

            // get the script controller
            CoinController coinController = coinObject.GetComponent<CoinController>();

            // set the target for the coin to fly to
            coinController.CoinCounterTarget = CoinCounterTarget;

            // set coin move speed
            coinController.moveSpeed = coinMoveSpeed;

            // set coin fly type
            coinController.flyEaseType = coinFlyEaseType;

            // set the function for calling when coin completes flight
            coinController.onCoinFlyComplete = consumeCoin;

            // start coin flight
            coinController.GoToCoinCounter();

        }

        // Add coin to player
        void consumeCoin(int coins){
            GameManager.instance.AddCoin(coins);
        }

        // Destroy a food that has been stolen
        public void DestroyFood(int index)
        {
            // does the food still exist?
            if(Items.Count > index)
            {

                //destroy it
                Destroy(Items[index]);

            }
        }

        // Get a random food from the list
        public int GetRandomFreeFoodIndex()
        {
            // make a list for foods that havn't already been selected
            List<int> freeFoodIndexes = new List<int>();

            // populate the list with all foods
            for (int i = 0; i < Items.Count; i++)
            {
                freeFoodIndexes.Add(i);
            }

            // remove from the list those foods that were already selected
            foreach (var item in TakenFoodIndexes)
            {
                freeFoodIndexes.Remove(item);
            }

            // choose a random food from the list
            int randomfoodToTake = Random.Range(0, freeFoodIndexes.Count);

            // the chosen food
            int freeFoodIndex = -1;

            // set the chosen food from the list with the random index
            freeFoodIndex = freeFoodIndexes[randomfoodToTake];

            // add food to already selected food list
            TakenFoodIndexes.Add(freeFoodIndex);

            // return selected food
            return freeFoodIndex;

        }
    }
}

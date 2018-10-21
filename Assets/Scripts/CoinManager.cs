using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    public class CoinManager : MonoBehaviour
    {

        // a target for the coins to go to
        [SerializeField] GameObject CoinCounterTarget;

        // a coin prefab for creating new coins
        [SerializeField] GameObject Coin;

        // the coin move speed
        [SerializeField] float coinMoveSpeed = 1f;

        // the ease fly type for the coins
        [SerializeField] LeanTweenType coinFlyEaseType = LeanTweenType.easeInOutCubic;

        public static CoinManager instance = null;

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

        /// <summary>
        /// Creates the coin.
        /// </summary>
        /// <param name="item">Item.</param>
        public void createCoin(GameObject item)
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

        /// <summary>
        /// Add coin to player
        /// </summary>
        /// <param name="coins">Coins.</param>
        void consumeCoin(int coins)
        {
            GameManager.instance.AddCoin(coins);
        }


    }
}

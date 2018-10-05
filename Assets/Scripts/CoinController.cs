using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{
    /// <summary>
    /// Coin handling
    /// </summary>
    public class CoinController : MonoBehaviour
    {

        // how much is a coin worth
        public int coinAmount = 1;

        // the coin counter on screen
        public GameObject CoinCounterTarget;

        // coin move speed towards target
        public float moveSpeed = 2f;

        // type of ease to fly at
        public LeanTweenType flyEaseType = LeanTweenType.easeInOutCubic;

        // use this to know when coin is at the target spot
        public UnityAction<int> onCoinFlyComplete;

        // spawn sound
        [SerializeField] AudioClip pickup;

        // finish sound
        [SerializeField] AudioClip drop;

        /// <summary>
        /// Goes to coin counter.
        /// </summary>
        public void GoToCoinCounter()
        {
            // play starting sound
            SoundManager.instance.PlaySingle(pickup);

            // move coin to target
            LeanTween.move(gameObject, CoinCounterTarget.transform, moveSpeed).setEase(flyEaseType).setOnComplete(onCompleteTween);
        }

        /// <summary>
        /// Ons the complete tween.
        /// </summary>
        void onCompleteTween()
        {
            // play end sound
            SoundManager.instance.PlaySingle(drop);

            // call whoever subscribed to this
            if (onCoinFlyComplete != null)
            {
                // send the coin amount
                onCoinFlyComplete(coinAmount);
            }

            // destroy coin
            Destroy(gameObject);
        }
    }
}

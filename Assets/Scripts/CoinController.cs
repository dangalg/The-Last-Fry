using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{
    public class CoinController : MonoBehaviour
    {

        public int coinAmount = 1;
        public GameObject CoinCounterTarget;
        public float moveSpeed = 2f;
        public LeanTweenType flyEaseType = LeanTweenType.easeInOutCubic;
        public UnityAction<int> onCoinFlyComplete;

        [SerializeField] AudioClip pickup;
        [SerializeField] AudioClip drop;


        public void GoToCoinCounter()
        {
            SoundManager.instance.PlaySingle(pickup);

            LeanTween.move(gameObject, CoinCounterTarget.transform, moveSpeed).setEase(flyEaseType).setOnComplete(onCompleteTween);
        }

        void onCompleteTween()
        {
            SoundManager.instance.PlaySingle(drop);
            if (onCoinFlyComplete != null)
            {
                onCoinFlyComplete(coinAmount);
            }
            Destroy(gameObject);
        }
    }
}

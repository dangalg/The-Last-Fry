using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinController : MonoBehaviour {

    public GameObject CoinCounterTarget;
    public Sprite Coin;
    public float moveSpeed = 2f;
    public LeanTweenType fryFlyEaseType = LeanTweenType.easeInOutCubic;
    public UnityAction onCoinFlyComplete;


    public void GoToCoinCounter()
    {
        LeanTween.move(gameObject, CoinCounterTarget.transform, moveSpeed).setEase(fryFlyEaseType).setOnComplete(onCompleteTween);
    }

    void onCompleteTween()
    {
        if (onCoinFlyComplete != null)
        {
            onCoinFlyComplete();
        }
        Destroy(gameObject);
    }
}

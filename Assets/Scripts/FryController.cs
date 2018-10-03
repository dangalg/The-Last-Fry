using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FryController : MonoBehaviour {


    public GameObject CoinCounterTarget;
    public Sprite Coin;
    public float moveSpeed = 2f;
    public LeanTweenType fryFlyEaseType = LeanTweenType.easeInOutCubic;
    public UnityAction onCoinFlyComplete;

    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    public void GoToCoinCounter(){

        sr.sprite = Coin;

        LeanTween.move(gameObject, CoinCounterTarget.transform, moveSpeed).setEase(fryFlyEaseType).setOnComplete(onCompleteTween);
    }

    void onCompleteTween(){
        if(onCoinFlyComplete != null){
            onCoinFlyComplete();
        }
        Destroy(gameObject);
    }
}

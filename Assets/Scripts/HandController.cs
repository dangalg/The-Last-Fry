﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{

    public class HandController : MonoBehaviour
    {

        public GameObject targetFry;
        public float moveSpeed = 3f;
        public int pointsForHit = 1;
        [SerializeField] Sprite openHand;
        [SerializeField] Sprite closedHand;

        public LeanTweenType handMovementType;

        [SerializeField] List<Sprite> hurtHands;
        public int fryIndex = 0;

        SpriteRenderer sr;

        Vector3 beginPosition;

        bool gotFry = false;
        bool handHit = false;

        int currentHit = 0;

        public UnityAction handGotFry;
        public UnityAction handGotHit;

        bool atemptingTheft = false;
        bool runningAway = false;

        int atemptTheftTween;
        int runAwayTween;

        // Use this for initialization
        void Start()
        {

            sr = GetComponentInChildren<SpriteRenderer>();

            beginPosition = transform.position;
            turnTowardsTarget(targetFry.transform.position);
        }

        // Update is called once per frame
        void Update()
        {

            if ((gotFry || handHit) && !runningAway)
            {
                runningAway = true;
                runAway();
            }

            if (!gotFry && !handHit)
            {
                if (transform.position != targetFry.transform.position)
                {
                    if(!atemptingTheft){
                        atemptingTheft = true;
                        atemptTheft();
                    }

                }
                else
                {
                    gotFry = true;
                    sr.sprite = closedHand;
                    targetFry.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            if (transform.position == beginPosition)
            {
                if (gotFry)
                {
                    // TODO lose life
                    FrySpawner.instance.DestroyFry(fryIndex);

                    if (handGotFry != null)
                    {
                        handGotFry();
                    }

                    handGotFry = null;

                    Destroy(gameObject);
                }

                if (handHit)
                {
                    if (handGotHit != null)
                    {
                        handGotHit();
                    }

                    handGotHit = null;

                    Destroy(gameObject);
                }
            }

        }

        public void HitHand()
        {
            handHit = true;
            if (currentHit < hurtHands.Count)
            {
                // Display next hurt hand... It gets worse and worse!
                sr.sprite = hurtHands[currentHit];
                currentHit++;

                if (currentHit == 1)
                {
                    GameManager.instance.AddPoint(pointsForHit);
                }

                if (gotFry)
                {
                    gotFry = false;
                    targetFry.GetComponent<SpriteRenderer>().enabled = true;
                    FrySpawner.instance.TakenFryIndexes.Remove(fryIndex);
                }
            }
        }

        private void moveTowardsTarget(Vector3 target)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                                                                     target, moveSpeed * Time.deltaTime);

            if (gotFry)
            {
                targetFry.transform.position = transform.position;
                targetFry.transform.rotation = transform.rotation;
            }
        }

        private void turnTowardsTarget(Vector3 target)
        {
            transform.right = target - transform.position;
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180);
        }

        private void atemptTheft()
        {
            turnTowardsTarget(targetFry.transform.position);
            atemptTheftTween = LeanTween.move(gameObject, targetFry.transform.position, moveSpeed).setEase(handMovementType).id;
        }

        private void runAway()
        {
            LeanTween.cancel(atemptTheftTween);
            runAwayTween = LeanTween.move(gameObject, beginPosition, moveSpeed).setEase(handMovementType).id;
        }
    }
}

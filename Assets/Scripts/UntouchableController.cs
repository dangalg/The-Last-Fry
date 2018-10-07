﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{
    public class UntouchableController : MonoBehaviour
    {
        // range in screen from center to float on
        public float DistanceToFloat = 1.7f;

        // The move speed
        public float moveSpeed = 3;

        // amount of life to lose when touching this
        public int lifeToLose = 1;

        // The index of untouchable in list.
        public int indexOfUntouchableInList = 0;

        // subscribe to this to know when flight has completed
        public UnityAction<int> onCompleteFloat;

        // Hit calback
        public UnityAction<int> onHitUntouchable;

        // the beginning point for the untouchable
        public Vector3 beginPoint;

        // the id for attempting theft tween in order to stop it in case the hand is hit
        int floatToTargetTweenId = 0;

        // untouchable collider to turn it off after hit
        PolygonCollider2D untouchableCollider;

        private void Start()
        {
            untouchableCollider = GetComponent<PolygonCollider2D>();
        }

        /// <summary>
        /// Floats to traget.
        /// </summary>
        /// <param name="targetPosition">Target position.</param>
        public void floatToTraget(Vector3 targetPosition)
        {
            // if it is already moving first stop it
            if(floatToTargetTweenId != 0)
            {
                LeanTween.cancel(floatToTargetTweenId);
            }

            // float to given position
            floatToTargetTweenId = LeanTween.move(gameObject, targetPosition, moveSpeed).setEase(LeanTweenType.animationCurve).setOnComplete(onComplete).id;

            // turn to target
            turnTowardsTarget(targetPosition);
        }

        /// <summary>
        /// Turns the towards target.
        /// </summary>
        /// <param name="target">Target.</param>
        private void turnTowardsTarget(Vector3 target)
        {
            // get the right from the target
            transform.right = target - transform.position;

            // spin the transform to face the target
            LeanTween.rotateZ(gameObject, transform.eulerAngles.z - 90f, 0.5f);
          
        }

        /// <summary>
        /// Ons the complete.
        /// </summary>
        void onComplete()
        {
            // call callback
            if(onCompleteFloat != null)
            {

                onCompleteFloat(indexOfUntouchableInList);

            }

        }

        public void onHit()
        {
            untouchableCollider.enabled = false;
            // call callback
            if(onHitUntouchable != null){

                // hit me
                onHitUntouchable(lifeToLose);
            }

            Destroy(gameObject, 1f);
        }

    }
}

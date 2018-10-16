using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{

    public class ThiefController : MonoBehaviour
    {

        // the food to steal
        public GameObject targetFood;

        // The callbacks
        public UnityAction thiefGotFood;
        public UnityAction thiefGotHit;

        // the index in the food list of the target food
        public int foodIndex = 0;

        // move speed
        public float moveSpeed = 3f;

        // points to player for hitting this hand
        public int pointsForHit = 1;

        // life to lose for player
        public int lifeForFoodStolen = 1;

        // hit to drop food on
        public int hitToDropFoodOn = 1;

        // Open hand image
        [SerializeField] Sprite openHand;

        // closed hand image
        [SerializeField] Sprite closedHand;

        // thief movement ease towards target
        public LeanTweenType handMovementType;

        // list of hurt images in order of good to worst
        [SerializeField] List<Sprite> hurtHands;

        // the sprite rnderer
        SpriteRenderer sr;

        // begin position target for stealing
        public Vector3 beginPosition;

        // end position target for run away
        public Vector3 endPosition;

        // do i have food?
        bool gotFood = false;

        // I am hit
        bool thiefHit = false;

        // I am attempting theft moving towards target
        bool atemptingTheft = false;

        //  I am running away with or without food
        bool runningAway = false;

        // I was hit # times
        int currentHit = 0;

        // the id for attempting theft tween in order to stop it in case the hand is hit
        int atemptTheftTween = 0;

        // Use this for initialization
        void Start()
        {
            // get the sprite renderer
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        /// <summary>
        /// Setup this instance.
        /// </summary>
        public void Setup(){
            // set position
            transform.position = beginPosition;

            // face target
            turnTowardsTarget(targetFood.transform.position);
        }

        // Update is called once per frame
        void Update()
        {

            // if I got foor or got hit and I am not already running away
            if ((gotFood || thiefHit) && !runningAway)
            {
                // set running away
                runningAway = true;

                // run!!!
                runAway();
            }

            // If i haven't stolen the food yet or gotten hit
            if (!gotFood && !thiefHit)
            {

                // if I haven't reached position yet
                if (transform.position != targetFood.transform.position)
                {

                    // if ihavn't attempted theft yet
                    if(!atemptingTheft){

                        // set attempting theft
                        atemptingTheft = true;

                        // Steal!!!
                        atemptTheft();
                    }

                }
                else
                {
                    // set got food 
                    gotFood = true;

                    // set image to closed hand
                    sr.sprite = closedHand;

                    // make food dissapear under hand
                    targetFood.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            // got to end position
            if (transform.position == endPosition)
            {
                // did I steal food?
                if (gotFood)
                {
                    // lose life for player
                    GameManager.instance.LoseLife(lifeForFoodStolen);
                    FoodSpawner.instance.DestroyFood(foodIndex);

                    // fire callback
                    if (thiefGotFood != null)
                    {
                        thiefGotFood();
                    }

                    // destroy thief
                    Destroy(gameObject);
                }

                // if the thief was hit
                if (thiefHit)
                {
                    // fire callback
                    if (thiefGotHit != null)
                    {
                        thiefGotHit();
                    }

                    // destroy thief
                    Destroy(gameObject);
                }
            }

        }

        /// <summary>
        /// Hits the hand.
        /// </summary>
        public void HitHand()
        {

            // set thief hit
            thiefHit = true;

            // if not all hand images have been used
            if (currentHit < hurtHands.Count)
            {
                // Display next hurt hand... It gets worse and worse!
                sr.sprite = hurtHands[currentHit];

                // up the image counter
                currentHit++;

                // first hit?
                if (currentHit == 1)
                {
                    // then add points
                    GameManager.instance.AddPoint(pointsForHit);

                    // make food available to other thiefs again
                    FoodSpawner.instance.TakenFoodIndexes.Remove(foodIndex);
                }

                // if the thief has a food
                if (gotFood)
                {
                    // set doesn't have food
                    gotFood = false;

                    // drop the food
                    targetFood.GetComponent<SpriteRenderer>().enabled = true;

                }
            }
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
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180);
        }

        /// <summary>
        /// Atempts the theft.
        /// </summary>
        private void atemptTheft()
        {
            // turn to target
            turnTowardsTarget(targetFood.transform.position);

            // move to target
            atemptTheftTween = LeanTween.move(gameObject, targetFood.transform.position, moveSpeed).setEase(handMovementType).id;
        }

        /// <summary>
        /// Runs the away.
        /// </summary>
        private void runAway()
        {
            // stop moving to target
            LeanTween.cancel(atemptTheftTween);

            // move to end position
            LeanTween.move(gameObject, endPosition, moveSpeed).setEase(handMovementType).setOnUpdate(onMovementUpdate);

        }

        /// <summary>
        /// callback for getting position of movement in tween
        /// </summary>
        /// <param name="obj">Object.</param>
        void onMovementUpdate(float obj)
        {
            // stole the food?
            if (gotFood)
            {
                // keep the food position and rotation same as mine during movement
                targetFood.transform.position = transform.position;
                targetFood.transform.rotation = transform.rotation;
            }
        }

    }
}

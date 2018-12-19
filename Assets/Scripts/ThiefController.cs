using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLastFry
{

    public class ThiefController : MonoBehaviour
    {
    
        public int ThiefLevel = 1;

        // The name of the item
        public string ThiefName;

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

        // coins for hitting thief
        public int minCoinsForHit = 1;

        // coins for hitting thief
        public int maxCoinsForHit = 1;

        // time between spawning coins
        public float timeBetweenCoinsToSpawn = 0.1f;

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

        // dead image
        [SerializeField] Sprite deadImage;

        // the sprite rnderer
        SpriteRenderer sr;

        // begin position target for stealing
        public Vector3 beginPosition;

        // end position target for run away
        public Vector3 endPosition;

        // fly when hit
        public bool flyWhenHit = false;

        // The speed to fly off screen when hit
        public float flyOffScreenSpeed = 1.5f;

        // sound effect when flying
        [SerializeField] List<AudioClip> flySounds;

        // turn towards target
        public bool shouldTurnTowardsTarget = true;

        // turn towards target angle
        public float turnTowardsAngle = 180f;

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

        // the id for attempting theft tween in order to stop it in case the hand is hit
        int runningAwayTween = 1;

        // rotate on death
        [SerializeField] bool rotateOnDeath;

        // fly off screen on death
        [SerializeField] bool flyOffScreenOnDeath;

        // sound effect when getting hit
        [SerializeField] List<AudioClip> hitSounds;

        // animate on death
        [SerializeField] bool animateOnDeath;

        Animator animator;

        // Use this for initialization
        void Start()
        {
            // get the sprite renderer
            sr = GetComponentInChildren<SpriteRenderer>();

            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Setup this instance.
        /// </summary>
        public void Setup()
        {
            // set position
            transform.position = beginPosition;

            // face target
            turnTowardsTarget(targetFood.transform.position);

            // if fly when hit 
            if (flyWhenHit)
            {

                // make end position opposite of start position so the character will grab the fry and run to the other side
                endPosition = new Vector3(endPosition.x * (-1), endPosition.y * (-1), endPosition.z);

                // play random fly sound
                int randomSoundIndex = Random.Range(0, flySounds.Count);
                AudioClip flySound = flySounds[randomSoundIndex];
                //SoundManager.instance.PlayFly(flySound);

            }

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

                    // if not fly when hit
                    if (!flyWhenHit)
                    {
                        // set image to closed hand
                        sr.sprite = closedHand;

                        // make food dissapear under hand
                        targetFood.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }

            // got to end position
            if (transform.position == endPosition)
            {
                IDied();
            }

        }

        private void IDied()
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

        /// <summary>
        /// Hits the hand.
        /// </summary>
        public void HitHand()
        {

            // set thief hit
            thiefHit = true;

            // stop running away
            runningAway = false;

            // play random hit sound
            int randomSoundIndex = Random.Range(0, hitSounds.Count);
            AudioClip hitSound = hitSounds[randomSoundIndex];
            SoundManager.instance.PlayHitSingle(hitSound);

            // if not all hand images have been used
            if (currentHit < hurtHands.Count)
            {
                // if not fly when hit
                if (!flyWhenHit)
                {
                    // Display next hurt hand... It gets worse and worse!
                    sr.sprite = hurtHands[currentHit];
                }
                else
                {
                    Die();

                }

                // up the image counter
                currentHit++;

                // first hit?
                if (currentHit == 1)
                {

                    // then add points
                    GameManager.instance.AddPoint(pointsForHit);

                    // make food available to other thiefs again
                    FoodSpawner.instance.TakenFoodIndexes.Remove(foodIndex);

                    // spawn coins for hit
                    StartCoroutine(SpawnCoinsAfterHandHit());
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
        /// Die this instance.
        /// </summary>
        private void Die()
        {
            if (animateOnDeath)
            {
                animator.SetTrigger("explode");
            }
            else
            {
                // stop animation
                animator.enabled = false;

                // show death image for flying object
                sr.sprite = deadImage;
            }
        }

        /// <summary>
        /// Spawns the coins after hand hit.
        /// </summary>
        /// <returns>The coins after hand hit.</returns>
        IEnumerator SpawnCoinsAfterHandHit(){

            // coins for hit
            int coinsForHit = 1;

            // get random coin amount by levelrotateOnDeath
            coinsForHit = Random.Range(minCoinsForHit, maxCoinsForHit + 1);

            for (int i = 0; i < coinsForHit; i++)
            {
                // create coin at thief position
                CoinManager.instance.createCoin(gameObject);

                // wait between coins
                yield return new WaitForSeconds(timeBetweenCoinsToSpawn);
            }
        }

        /// <summary>
        /// Turns the towards target.
        /// </summary>
        /// <param name="target">Target.</param>
        private void turnTowardsTarget(Vector3 target)
        {
            if (shouldTurnTowardsTarget)
            {
                // get the right from the target
                transform.right = target - transform.position;

                // spin the transform to face the target
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + turnTowardsAngle);
            }

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



            if (thiefHit && rotateOnDeath)
            {
                // spin the transform
                LeanTween.rotateZ(gameObject, 1024, flyOffScreenSpeed);
            }

            if (thiefHit)
            {

                // stop running away
                LeanTween.cancel(runningAwayTween);

                if (flyOffScreenOnDeath)
                {
                    // move to end position
                    LeanTween.move(gameObject, endPosition, moveSpeed * 0.3f).setEase(handMovementType).setOnUpdate(onMovementUpdate).setOnComplete(onCompleteFlight);
                }
                else
                {
                    Invoke("IDied",2f);
                }

            }
            else
            {
                if (runningAway)
                {
                    // move to end position
                    runningAwayTween = LeanTween.move(gameObject, endPosition, moveSpeed * 0.3f).setEase(handMovementType).setOnUpdate(onMovementUpdate).setOnComplete(onCompleteFlight).id;
                }
            }

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

        void onCompleteFlight()
        {
            // if fly when hit 
            if (flyWhenHit)
            {
                // stop fly music
                //SoundManager.instance.StopFly();
            }
        }

    }
}

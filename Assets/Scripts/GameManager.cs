using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheLastFry
{

    public class GameManager : MonoBehaviour
    {


        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
         
        // The texts on screen
        [SerializeField] TMP_Text coinsText;
        [SerializeField] TMP_Text levelText;
        [SerializeField] TMP_Text lifeText;

        // the buttons on screen
        [SerializeField] Button AdButton;
        [SerializeField] Button CoinButton;

        // the button texts
        [SerializeField] Text AdButtonText;
        [SerializeField] Text CoinButtonText;

        // The panels on screen
        [SerializeField] GameObject ContinuePanel;

        // the player data
        PlayerData playerData;

        // coins to pay to continue next level
        public int coinsForNextLevel = 50;

        // life to give player back for continueing 
        public int lifeForContinue = 1;

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

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        /// <summary>
        /// Inits the game.
        /// </summary>
        void InitGame()
        {
            // load the player data
            playerData = DataHandler.LoadPlayerData();

            // set points to zero -- used to calculate when the level ends
            playerData.Points = 0;

            // display the stats
            displayStats();

            // show the coins
            CoinButtonText.text = coinsForNextLevel.ToString() + " Coins";

            // setup the game
            StartCoroutine(SetupGame(playerData.Level));

        }

        private void Start()
        {
            // sign in to ad manager callbacks
            AdManager.instance.onSkippedAd = DecideEndorContinue;
            AdManager.instance.onFailedAd = DecideEndorContinue;
            AdManager.instance.onFinishedAd = ContinueAd;

        }



        IEnumerator SetupGame(int level)
        {
            Debug.Log("lastfry : Setup Game");
            
            //enabled level text
            levelText.enabled = true;

            // display level text
            levelText.text = level.ToString();

            // set player points to 0
            playerData.Points = 0;

            // display stats
            displayStats();

            yield return new WaitForSeconds(2f);

            // disable level text
            levelText.enabled = false;

            Debug.Log("lastfry : Reseting spawners");

            // reset the spawners
            FoodSpawner.instance.Reset();
            ThiefSpawner.instance.Reset();
            UntouchableSpawner.instance.Reset();

            Debug.Log("lastfry : Reset spawners");

            // setup food spawner
            FoodSpawner.instance.Setup(playerData.Level);

            Debug.Log("lastfry : setup food spawner");

            yield return new WaitForSeconds(1f);

            // set up thief spawner
            ThiefSpawner.instance.Setup(playerData.Level);

            Debug.Log("lastfry : setup thief spawner");

            // set up thief spawner
            UntouchableSpawner.instance.Setup(playerData.Level);

            Debug.Log("lastfry : setup untouchable spawner");
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                TryToHit(Input.mousePosition);
            }
#else
        if (Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase != TouchPhase.Ended){
                TryToHit(Input.GetTouch(0).position);
            } 
        }
#endif
        }

        /// <summary>
        /// Hits the hand.
        /// </summary>
        /// <param name="hitPosition">Hit position.</param>
        private void TryToHit(Vector3 hitPosition)
        {
            // get the hit hand position on screen
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(hitPosition), Vector2.zero);

            //Converting Mouse Pos to 2D (vector2) World Pos
            if (hit.collider != null)
            {

                // Am I hitting a thief?
                if (hit.collider.CompareTag("ThiefHand"))
                {
                    // call hit hand function
                    hit.collider.gameObject.GetComponent<ThiefController>().HitHand();

                }

                // Am I hitting an Untouchable?
                if (hit.collider.CompareTag("Untouchable"))
                {
                    // call hit hand function
                    UntouchableController controller = hit.collider.gameObject.GetComponent<UntouchableController>();

                    // lose life
                    controller.onHitUntouchable(controller.lifeToLose);

                }
            }
        }

        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="pointsToAdd">Points to add.</param>
        public void AddPoint(int pointsToAdd)
        {
            playerData.Points += pointsToAdd;
        }

        /// <summary>
        /// Subtracts the points.
        /// </summary>
        /// <param name="pointsToAdd">Points to add.</param>
        public void SubtractPoints(int pointsToAdd)
        {
            playerData.Points -= pointsToAdd;
        }

        /// <summary>
        /// Gains a life.
        /// </summary>
        /// <param name="lifeToAdd">Life to add.</param>
        public void GainLife(int lifeToAdd)
        {
            playerData.Life += lifeToAdd;
            displayStats();
        }

        /// <summary>
        /// Loses a life.
        /// </summary>
        /// <param name="lifeToLose">Life to lose.</param>
        public void LoseLife(int lifeToLose)
        {
            // lose life
            playerData.Life -= lifeToLose;

            // has my life ended?
            if (playerData.Life <= 0){

                // make life 0 if it is less for display purposes
                playerData.Life = 0;

                // go to continue screen
                DecideEndorContinue();
            }

            // display stats
            displayStats();
        }

        /// <summary>
        /// Displaies the stats.
        /// </summary>
        private void displayStats()
        {
            // display coins
            coinsText.text = playerData.Coins.ToString();

            // display life
            lifeText.text = playerData.Life.ToString();
        }

        /// <summary>
        /// Open the Continue panel to decide if to end or continue.
        /// </summary>
        public void DecideEndorContinue()
        {
            // stop game
            StopGame();

            // display continue panel
            ContinuePanel.SetActive(true);
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        void StopGame(){

            // reset the spawners
            FoodSpawner.instance.Reset();
            ThiefSpawner.instance.Reset();
            UntouchableSpawner.instance.Reset();

        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            StartCoroutine(EndGameRoutine());
        }

        /// <summary>
        /// Ends the game routine.
        /// </summary>
        /// <returns>The game routine.</returns>
        IEnumerator EndGameRoutine(){
        
            // load main menu
            SceneManager.LoadScene("MainMenu");

            yield return null;
        }

        /// <summary>
        /// Go to Next level.
        /// </summary>
        public IEnumerator NextLevel()
        {
            // stop the game but do not reset food spawner because the fries are needed
            ThiefSpawner.instance.Reset();
            UntouchableSpawner.instance.Reset();

            // give the coins
            yield return StartCoroutine(FoodSpawner.instance.NextLevelRoutine());

            // up the level for player
            playerData.Level++;

            // start level
            StartLevel();

        }

        /// <summary>
        /// Starts the level.
        /// </summary>
        private void StartLevel()
        {
            // save player data
            DataHandler.SavePlayerData(playerData);

            // start the game
            StartCoroutine(SetupGame(playerData.Level));
        }

        /// <summary>
        /// Adds coin to player.
        /// </summary>
        /// <param name="coins">Coins.</param>
        public void AddCoin(int coins){

            // add coin to player
            playerData.Coins += coins;

            // display stats
            displayStats();
        }

        /// <summary>
        /// Removes coin from the player.
        /// </summary>
        /// <param name="coins">Coins.</param>
        public void LoseCoin(int coins)
        {
            // remove coins
            playerData.Coins -= coins;

            // display stats
            displayStats();
        }

        /// <summary>
        /// Continues the coins.
        /// </summary>
        public void ContinueCoins()
        {
            // hide continue panel
            ContinuePanel.SetActive(false);

            // remove coins from player
            LoseCoin(coinsForNextLevel);

            // give player life back
            playerData.Life = lifeForContinue;

            // start level
            StartLevel();
        }

        /// <summary>
        /// Watch an add.
        /// </summary>
        public void WatchAdd(){
            AdManager.instance.ShowAd();
        }

        /// <summary>
        /// Continues after ad.
        /// </summary>
        public void ContinueAd()
        {
            // hide continue panel
            ContinuePanel.SetActive(false);

            // give player life back
            playerData.Life = lifeForContinue;

            // start level
            StartLevel();
        }

    }
}
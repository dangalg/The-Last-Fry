using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] Button ExitButton;

        // Yes No buttons and text
        [SerializeField] Button YesButton;
        [SerializeField] Button NoButton;
        [SerializeField] TMP_Text YesNoText;

        // the shop buttons
        [SerializeField] GameObject ShopPanel;

        // the button texts
        [SerializeField] Text AdButtonText;
        [SerializeField] Text CoinButtonText;
        [SerializeField] TMP_Text Message;

        // The panels on screen
        [SerializeField] GameObject ContinuePanel;
        [SerializeField] GameObject MessagePanel;
        [SerializeField] GameObject YesNoPanel;

        // the player data
        PlayerData playerData;

        // coins to pay to continue next level
        public int coinsForNextLevel = 50;

        // life to give player back for continueing 
        public int lifeForContinue = 1;

        // level for testing
        public int levelForTesting = 0;

        // Audio game music
        [SerializeField] AudioClip GameMusic;

        // Audio win jingle
        [SerializeField] AudioClip WinSound;

        // Audio lose jingle
        [SerializeField] AudioClip LoseSound;

        // coin from screen on top bar
        [SerializeField] GameObject coinOnTopBar;

        // audio sound for coin
        [SerializeField] AudioClip coinSound;

        // a list of saying that show at end of level
        [SerializeField] List<string> sayings = new List<string>();

        // id for coin bounce tween
        int coinBounceTween = 12;
        // id for coin bounce on complete tween
        int coinBounceOnCompleteTween = 13;

        // coin initial height
        float coinInitialY = 0;

        // make message dissapear on click
        bool messagePanelCloseOnClick = true;

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

            // set level for testing
            if(levelForTesting > 0)
            {
                playerData.Level = levelForTesting;
            }

            // set points to zero -- used to calculate when the level ends
            playerData.Points = 0;

            // display the stats
            displayStats();

            // hide continue panel
            ContinuePanel.SetActive(false);

            // hide shop panel
            ShopPanel.SetActive(false);

            // show the coins
            CoinButtonText.text = coinsForNextLevel.ToString() + " Coins";

        }

        private void Start()
        {

            coinInitialY = coinOnTopBar.transform.localPosition.y + 20;

            //PlayerPrefs.DeleteAll();

            //playerData.Coins = 1000000;
            //DataHandler.SavePlayerData(playerData);

            DataHandler.SaveIntToDB(Constants.PLAYEDFIRSTTIME, 1);

            // unpause game
            Time.timeScale = 1;

            // sign in to ad manager callbacks
            AdManager.instance.onSkippedAd = OpenContinuePanel;
            AdManager.instance.onFailedAd = OpenContinuePanel;
            AdManager.instance.onFinishedAd = ContinueAd;

            // sign in to shopManager
            ShopManager.instance.onPurchaseCoinsAction += onPurchaseCoins;
            ShopManager.instance.onPurchaseFailedAction += onPurchaseFailed;

            // setup the game
            StartCoroutine(SetupGame(playerData.Level));
        }

        private void OnDestroy()
        {
            if(AdManager.instance != null)
            {
                AdManager.instance.onSkippedAd = null;
                AdManager.instance.onFailedAd = null;
                AdManager.instance.onFinishedAd = null;
            }

            if(ShopManager.instance != null)
            {
                ShopManager.instance.onPurchaseCoinsAction -= onPurchaseCoins;
                ShopManager.instance.onPurchaseFailedAction -= onPurchaseFailed;
            }

        }

        void onPurchaseCoins(ShopManager.CoinAmount coinAmount)
        {
            // give gems to player TODO make exciting animation
            switch (coinAmount)
            {
                case ShopManager.CoinAmount.HANDFUL:
                    playerData.Coins += 500;
                    break;
                case ShopManager.CoinAmount.PILE:
                    playerData.Coins += 1200;
                    break;
                case ShopManager.CoinAmount.SACK:
                    playerData.Coins += 4000;
                    break;
                case ShopManager.CoinAmount.BAG:
                    playerData.Coins += 6500;
                    break;
                case ShopManager.CoinAmount.CHEST:
                    playerData.Coins += 10000;
                    break;
            }

            // save player data
            DataHandler.SavePlayerData(playerData);

            // update coins text
            displayStats();

            // hide shop panel
            ShopPanel.SetActive(false);

            ContinuePanel.SetActive(true);

            // notify user of something wrong with purchase
            Message.text = "Thank You For The Purchase!";
            MessagePanel.SetActive(true);
        }

        /// <summary>
        /// Clicks the message panel.
        /// </summary>
        public void clickMessagePanel()
        {
            if (messagePanelCloseOnClick)
            {
                // close message panel
                MessagePanel.SetActive(false);
            }

        }

        /// <summary>
        /// Clicks the exit button.
        /// </summary>
        public void ClickExitButton()
        {

            // pause game
            Time.timeScale = 0;

            // set yes no panel text
            YesNoText.text = "Leave Game?";

            // open Yes No panel
            YesNoPanel.SetActive(true);
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public void ExitGame()
        {
            // stop game music
            SoundManager.instance.StopMusic();

            // end game
            EndGame();
        }

        /// <summary>
        /// Closes the yes no panel.
        /// </summary>
        public void closeYesNoPanel()
        {
            // close Yes No panel
            YesNoPanel.SetActive(false);

            // unpause game
            Time.timeScale = 1;
        }

        /// <summary>
        /// Closes the shop panel.
        /// </summary>
        public void closeShopPanel()
        {
            // close message panel
            ShopPanel.SetActive(false);
            ContinuePanel.SetActive(true);
        }

        /// <summary>
        /// Ons the purchase failed.
        /// </summary>
        void onPurchaseFailed()
        {

            // notify user of something wrong with purchase
            Message.text = "Something Went Wrong Please Try Again Later..";
            MessagePanel.SetActive(true);
        }

        IEnumerator SetupGame(int level)
        {
        
            Debug.Log("LASTFRY: reset the spawners");
            // reset the spawners
            Debug.Log("LASTFRY: reset ThiefSpawner");
            ThiefSpawner.instance.Reset();
            Debug.Log("LASTFRY: reset UntouchableSpawner");
            UntouchableSpawner.instance.Reset();
            Debug.Log("LASTFRY: reset OneUpSpawner");
            OneUpSpawner.instance.Reset();

            Debug.Log("LASTFRY: enabled level text");
            //enabled level text
            levelText.enabled = true;

            Debug.Log("LASTFRY: display level text");
            // display level text
            levelText.text = level.ToString();

            Debug.Log("LASTFRY: set player points to 0");
            // set player points to 0
            playerData.Points = 0;

            Debug.Log("LASTFRY: display stats");
            // display stats
            displayStats();

            yield return new WaitForSeconds(2f);

            Debug.Log("LASTFRY: reset FoodSpawner");
            FoodSpawner.instance.Reset();

            Debug.Log("LASTFRY: disable level text");
            // disable level text
            levelText.enabled = false;

            Debug.Log("LASTFRY: setup food spawner");

            // setup food spawner
            FoodSpawner.instance.Setup(playerData.Level);

            yield return new WaitForSeconds(1f);

            Debug.Log("LASTFRY: setup thief spawner");
            // set up thief spawner
            ThiefSpawner.instance.Setup(playerData.Level);

            Debug.Log("LASTFRY: setup untouchable spawner");
            // set up untouchable spawner
            UntouchableSpawner.instance.Setup(playerData.Level);

            Debug.Log("LASTFRY: setup one up spawner");
            // set up one up spawner
            OneUpSpawner.instance.Setup(playerData.Level);

            Debug.Log("LASTFRY: play game music");
            // play game music
            SoundManager.instance.PlayMusic(GameMusic);

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
            // get the hit hand position on screecoinInitialYcoinInitialYn
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
                    controller.onHit();

                }

                // Am I hitting an One Up?
                if (hit.collider.CompareTag("OneUp"))
                {
                    // call hit hand function
                    OneUpController controller = hit.collider.gameObject.GetComponent<OneUpController>();

                    // gain life
                    controller.onHit();

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
                OpenContinuePanel();
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
        public void OpenContinuePanel()
        {

            // stop game music
            SoundManager.instance.StopMusic();

            // play Lose sound
            SoundManager.instance.PlayHitSingle(LoseSound);

            // stop game
            StopGame();

            // display continue panel
            ContinuePanel.SetActive(true);

            // Save Data
            DataHandler.SavePlayerData(playerData);
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        void StopGame(){

            // reset the spawners
            FoodSpawner.instance.Reset();
            ThiefSpawner.instance.Reset();
            UntouchableSpawner.instance.Reset();
            OneUpSpawner.instance.Reset();
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

        public void GoToNextLevel()
        {

            // stop game music
            SoundManager.instance.StopMusic();

            // play win sound
            SoundManager.instance.PlayHitSingle(WinSound);

            if (PlayerPrefs.GetInt(Constants.HIGHESTLEVEL) < playerData.Level)
            {
                PlayerPrefs.SetInt(Constants.HIGHESTLEVEL, playerData.Level);
            }

            // start next level
            StartCoroutine(NextLevel());
        }

        /// <summary>
        /// Go to Next level.
        /// </summary>
        public IEnumerator NextLevel()
        {

            // get random message
            int randomMessageIndex = Random.Range(0, sayings.Count-1);

            // show message panel with random message
            Message.text = sayings[randomMessageIndex];
            messagePanelCloseOnClick = false;
            MessagePanel.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            // hide message text
            MessagePanel.SetActive(false);
            messagePanelCloseOnClick = true;

            // stop the game but do not reset food spawner because the fries are needed
            ThiefSpawner.instance.Reset();
            UntouchableSpawner.instance.Reset();
            OneUpSpawner.instance.Reset();

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

            LeanTween.cancel(coinBounceTween);
            LeanTween.cancel(coinBounceOnCompleteTween);

            // make coin bounce
            coinBounceTween = LeanTween.moveLocalY(coinOnTopBar, coinInitialY+20f, 0.25f).setOnComplete(() =>{
                coinBounceOnCompleteTween = LeanTween.moveLocalY(coinOnTopBar, coinInitialY -20f, 0.25f).setEaseOutBounce().id;
              }).id;

            // play coin sound
            SoundManager.instance.PlaySingle(coinSound);
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

            // check if player has enough coins
            bool playerHasEnoughCoins = playerData.Coins >= coinsForNextLevel;

            if (playerHasEnoughCoins)
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
            else
            {
                // hide continue panel
                ContinuePanel.SetActive(false);

                // show message not enough coins
                ShopPanel.SetActive(true);
            }
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

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyHandfulOfCoins()
        {
            ShopManager.instance.BuyHandfulOfCoins();
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyPileOfCoins()
        {
            ShopManager.instance.BuyPileOfCoins();
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuySackOfCoins()
        {
            ShopManager.instance.BuySackOfCoins();
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyBagOfCoins()
        {
            ShopManager.instance.BuyBagOfCoins();
        }

        /// <summary>
        /// Buy the handful of gems.
        /// </summary>
        public void BuyChestOfCoins()
        {
            ShopManager.instance.BuyChestOfCoins();
        }

    }
}
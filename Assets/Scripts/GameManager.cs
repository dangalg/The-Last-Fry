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
         
        [SerializeField] TMP_Text coinsText;
        [SerializeField] TMP_Text levelText;
        [SerializeField] TMP_Text lifeText;

        [SerializeField] Button AdButton;
        [SerializeField] Button CoinButton;
        [SerializeField] Text AdButtonText;
        [SerializeField] Text CoinButtonText;

        [SerializeField] GameObject ContinuePanel;


        [SerializeField] float percentToPassLevel = 70f;

        PlayerData playerData;

        public int coinsForNextLevel = 50;

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

        //Initializes the game for each level.
        void InitGame()
        {
            playerData = DataHandler.LoadPlayerData();

            playerData.Points = 0;
            displayStats();

            StartCoroutine(SetupGame(playerData.Level));

            CoinButtonText.text = coinsForNextLevel.ToString() + " Coins";

        }

        private void Start()
        {
            AdManager.instance.onSkippedAd = DecideEndorContinue;
            AdManager.instance.onFailedAd = DecideEndorContinue;
            AdManager.instance.onFinishedAd = ContinueAd;

        }



        IEnumerator SetupGame(int level)
        {

            yield return FoodSpawner.instance.NextLevelRoutine();

            levelText.enabled = true;

            levelText.text = level.ToString();

            playerData.Points = 0;

            displayStats();

            yield return new WaitForSeconds(2f);

            levelText.enabled = false;

            FoodSpawner.instance.Reset();
            ThiefSpawner.instance.Reset();
            //UntouchableSpawner.instance.Reset();

            FoodSpawner.instance.Setup(playerData.Level);

            yield return new WaitForSeconds(1f);

            ThiefSpawner.instance.Setup(playerData.Level);
           //UntouchableSpawner.instance.Setup(playerData.Level);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                HitHand(Input.mousePosition);
            }
#else
        if (Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase != TouchPhase.Ended){
                HitHand(Input.GetTouch(0).position);
            } 
        }
#endif
        }

        private void HitHand(Vector3 hitPosition)
        {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(hitPosition), Vector2.zero);

            //Converting Mouse Pos to 2D (vector2) World Pos
            if (hit.collider != null)
            {

                if (hit.collider.CompareTag("ThiefHand"))
                {
                    hit.collider.gameObject.GetComponent<ThiefController>().HitHand();

                }
            }
        }

        public void AddPoint(int pointsToAdd)
        {
            playerData.Points += pointsToAdd;
        }

        public void SubtractPoints(int pointsToAdd)
        {
            playerData.Points -= pointsToAdd;
        }

        public void GainLife(int lifeToAdd)
        {
            playerData.Life += lifeToAdd;
            displayStats();
        }

        public void LoseLife(int lifeToLose)
        {
            playerData.Life -= lifeToLose;

            if (playerData.Life <= 0){
                playerData.Life = 0;
                DecideEndorContinue();
            }

            displayStats();
        }

        private void displayStats()
        {
            coinsText.text = playerData.Coins.ToString();
            lifeText.text = playerData.Life.ToString();
        }

        public void DecideEndorContinue(){

            StopGame();
            ContinuePanel.SetActive(true);
        }

        void StopGame(){
            FoodSpawner.instance.Reset();
            ThiefSpawner.instance.Reset();
        }

        public void EndGame()
        {
            StartCoroutine(EndGameRoutine());
        }

        IEnumerator EndGameRoutine(){
        
            SceneManager.LoadScene("MainMenu");

            yield return null;
        }

        public void NextLevel()
        {

            playerData.Level++;
            StartLevel();

        }

        private void StartLevel()
        {
            DataHandler.SavePlayerData(playerData);

            StartCoroutine(SetupGame(playerData.Level));
        }

        public void AddCoin(int coins){
            playerData.Coins += coins;
            displayStats();
        }

        public void LoseCoin(int coins)
        {
            playerData.Coins -= coins;
            displayStats();
        }

        public void ContinueCoins(){
            ContinuePanel.SetActive(false);
            LoseCoin(coinsForNextLevel);
            playerData.Life = 1;
            StartLevel();
        }

        public void WatchAdd(){
            AdManager.instance.ShowAd();
        }

        public void ContinueAd()
        {
            ContinuePanel.SetActive(false);
            playerData.Life = 1;
            StartLevel();
        }

    }
}
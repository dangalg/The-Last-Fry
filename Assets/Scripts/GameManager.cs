using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace TheLastFry
{

    public class GameManager : MonoBehaviour
    {


        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
         
        [SerializeField] TMP_Text pointsText;
        [SerializeField] TMP_Text levelText;
        [SerializeField] TMP_Text lifeText;

        [SerializeField] float percentToPassLevel = 70f;

        PlayerData playerData;


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

        }

        IEnumerator SetupGame(int level)
        {

            levelText.enabled = true;

            levelText.text = level.ToString();

            playerData.Points = 0;
            playerData.Life = 3;

            displayStats();

            yield return new WaitForSeconds(2f);

            levelText.enabled = false;

            FrySpawner.instance.Reset();
            HandSpawner.instance.Reset();
            //UntouchableSpawner.instance.Reset();

            FrySpawner.instance.Setup(playerData.Level);

            yield return new WaitForSeconds(1f);

            HandSpawner.instance.Setup(playerData.Level);
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
                Debug.Log("Something Hit");

                if (hit.collider.CompareTag("ThiefHand"))
                {
                    hit.collider.gameObject.GetComponent<HandController>().HitHand();

                }
            }
        }

        public void AddPoint(int pointsToAdd)
        {
            playerData.Points += pointsToAdd;
            displayStats();
        }

        public void SubtractPoints(int pointsToAdd)
        {
            playerData.Points -= pointsToAdd;
            displayStats();
        }

        private void displayStats()
        {
            pointsText.text = playerData.Points.ToString();
            lifeText.text = playerData.Life.ToString();
        }

        public void EndGame()
        {
            if(!playerData.RemoveAds){
                AdManager.instance.PlayAdvertisement();
            }

            SceneManager.LoadScene("MainMenu");
        }

        public void NextLevel()
        {

            Debug.Log("EndLevel");

            bool endGame = levelCalculation();

            if (!endGame)
            {

                DataHandler.SavePlayerData(playerData);

                StartCoroutine(SetupGame(playerData.Level));

            }
        }

        bool levelCalculation()
        {

            bool endGame = false;

            int level = playerData.Level;

            float percentOfFriesSaved = (((float)playerData.Points) / ((float)level)) * 100f;

            if (percentOfFriesSaved >= percentToPassLevel)
            {
                playerData.Level++;
            }
            else
            {
                endGame = true;
                EndGame();
            }

            return endGame;

        }

    }
}
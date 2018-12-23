using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace TheLastFry 
{

    public class AdManager : MonoBehaviour
    {

        public static AdManager instance = null;

        [SerializeField] string gameID = "2822484";

        // Callbacks
        public UnityAction onFinishedAd;
        public UnityAction onSkippedAd;
        public UnityAction onFailedAd;

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
            DontDestroyOnLoad(gameObject);
            Advertisement.Initialize(gameID, true);

        }


        /// <summary>
        /// Shows the ad.
        /// </summary>
        /// <param name="zone">The type of ad</param>
        public void ShowAd(string zone = "rewardedVideo")
        {
#if UNITY_EDITOR
            StartCoroutine(WaitForAd());
#endif

            if (string.Equals(zone, ""))
                zone = null;

            ShowOptions options = new ShowOptions();
            options.resultCallback = AdCallbackhandler;

            // start the advertisement
            if (Advertisement.IsReady(zone))
                Advertisement.Show(zone, options);
        }

        /// <summary>
        /// callbackhandler for the ad
        /// </summary>
        /// <param name="result">Result.</param>
        void AdCallbackhandler(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("Ad Finished. Rewarding player...");
                    if (onFinishedAd != null) onFinishedAd();
                    break;
                case ShowResult.Skipped:
                    Debug.Log("Ad skipped. Son, I am dissapointed in you");
                    if (onSkippedAd != null) onSkippedAd();
                    break;
                case ShowResult.Failed:
                    Debug.Log("I swear this has never happened to me before");
                    if (onFailedAd != null) onFailedAd();
                    break;
            }
        }

        /// <summary>
        /// Waits for ad in the unity editor
        /// </summary>
        /// <returns>The for ad.</returns>
        IEnumerator WaitForAd()
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            yield return null;

            while (Advertisement.isShowing)
                yield return null;

            Time.timeScale = currentTimeScale;
        }
    }
}

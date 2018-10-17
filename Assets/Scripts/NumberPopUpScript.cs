using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheLastFry
{
    public class NumberPopUpScript : MonoBehaviour
    {
    
        // the text number
        [SerializeField] TMP_Text numberText;

        /// <summary>
        /// Spawns the number.
        /// </summary>
        /// <param name="numberToShow">Number to show.</param>
        public void SpawnNumber(string numberToShow){

            // set sorting layer
            gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Foreground";

            // set number
            numberText.text = numberToShow;

            // float number up
            LeanTween.move(gameObject, new Vector2(transform.position.x, transform.position.y +2) , 1f).setEaseOutExpo().setOnComplete(completeFlight);
        }

        /// <summary>
        /// Completes the flight.
        /// </summary>
        void completeFlight(){
            Destroy(gameObject,0.1f);
        }
    }
}

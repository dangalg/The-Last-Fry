using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    [Serializable]
    public class ThiefType
    {

        /// <summary>
        /// The odds to get each player type
        /// </summary>
        /// <value>The odds.</value>
        public int Odds
        {
            get;
            set;
        }

        /// <summary>
        /// The thief type objects
        /// </summary>
        /// <value>The hand type object.</value>
        public GameObject ThiefTypeObject
        {
            get;
            set;
        }

        public ThiefType()
        {
            ThiefTypeObject = new GameObject();
        }
    }
}

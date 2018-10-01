using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    [Serializable]
    public class HandType
    {


        public int Odds
        {
            get;
            set;
        }
        public GameObject HandTypeObject
        {
            get;
            set;
        }

        public HandType()
        {
            HandTypeObject = new GameObject();
        }
    }
}

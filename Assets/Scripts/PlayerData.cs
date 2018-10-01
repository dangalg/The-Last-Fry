using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastFry
{
    [Serializable]
    public class PlayerData
    {

        private int energy;
        private int points;
        private int record;
        private int level;
        private int life;
        private bool removeAds;

        public int Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public int Record
        {
            get { return record; }
            set { record = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        public bool RemoveAds
        {
            get { return removeAds; }
            set { removeAds = value; }
        }

        public PlayerData()
        {
            energy = 3;
            points = 0;
            record = 0;
            level = 1;
            life = 3;
            removeAds = false;
        }

    }
}

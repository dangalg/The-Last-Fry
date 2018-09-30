using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {

    private int energy;
    private int points;
    private int level;

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

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public PlayerData()
    {
        energy = 3;
        points = 0;
        level = 1;
    }

}

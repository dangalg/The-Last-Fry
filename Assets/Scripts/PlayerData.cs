using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {

    public int Energy
    {
        get;
        set;
    }

    public int Record
    {
        get;
        set;
    }

    public PlayerData()
    {
        Energy = 3;
        Record = 0;
    }

}

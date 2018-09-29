using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

    const string SCORE = "score";

    public int Score
    {
        get{
            return DataHandler.LoadIntFromDB(SCORE);
        }
        set{
            DataHandler.SaveIntToDB(SCORE, value);
        }
    }
    
}

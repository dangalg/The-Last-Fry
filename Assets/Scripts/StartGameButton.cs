using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour {


    private Button startButton;

    private void Start()
    {
        startButton = GetComponent<Button>();

        startButton.onClick.AddListener(LoadGame);
    }

    public void LoadGame(){
        MainMenu.instance.DecreaseEnergy(1);
        SceneManager.LoadScene("Game");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Text highscoreText;

    void Start()
    {
        int highScore = PlayerPrefs.GetInt("player.highscore");
        highscoreText.text = $"High Score: {highScore}";
    }

    public void OnPlayButtonClicked()
    {
        //TODO: Load the scene in a more elegant way (?)
        SceneManager.LoadScene("Game");
    }
}

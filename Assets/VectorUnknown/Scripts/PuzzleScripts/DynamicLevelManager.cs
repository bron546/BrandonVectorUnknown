using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicLevelManager : MonoBehaviour
{
    /* 6/5/2020
    * Brandon Bayles
    * This script is designed to provide intermediary control to the UFO_PuzzleManager in order to control what the next level looks like. Instead of relying on the user to select a level from the UI
    * this scipt provides functionality for the next level to be chosen automatically with varying components. Right now this generation will be random but it will ultimately be controlled by a baysian net. 
    */

   // public UFO_PuzzleManager gameManager;
    public int levelNum = 0;
    public int attempt_count = 0;
    public int display_upcoming_path = 0;
    public int display_past_paths = 0;
    public int load_game_mode = 0;
    public bool tutorial = false;
    [Range(0, 7)]
    public int level_number;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
    public void level_one()
    {
        Debug.Log("Loading level 1 with difficulty: " + GameConstants.difficulty);
        Psychometrics.logEvent("L1");
        puzzle_settings(-1, 1, 1, 0);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadScene("VectorGame");
    }

    public void level_two()
    {
        Debug.Log("Loading level 2 with difficulty: " + GameConstants.difficulty);
        Psychometrics.logEvent("L2");
        puzzle_settings(-1, 0, 1, 0);
        PlayerPrefs.SetInt("CurrentLevel", 2);
        SceneManager.LoadScene("VectorGame");
    }

    public void level_three()
    {
        Debug.Log("Loading level 3 with difficulty: " + GameConstants.difficulty);
        Psychometrics.logEvent("L3");
        puzzle_settings(-1, 1, 1, 1);
        PlayerPrefs.SetInt("CurrentLevel", 3);
        SceneManager.LoadScene("VectorGame");
    }

    public void startLevel()//builds a level based on the provided int
    {
        switch (levelNum)
        {
            case 0:
                GameConstants.difficulty = 1;
                level_one();
                break;
            case 1:
                GameConstants.difficulty = 1;
                level_two();
                break;
            case 2:
                GameConstants.difficulty = 1;
                level_three();
                break;
            case 3:
                GameConstants.difficulty = 2;
                level_one();
                break;
            case 4:
                GameConstants.difficulty = 2;
                level_two();
                break;
            case 5:
                GameConstants.difficulty = 2;
                level_three();
                break;
            case 6:
                GameConstants.difficulty = 3;
                level_one();
                break;
            case 7:
                GameConstants.difficulty = 3;
                level_two();
                break;
            case 8:
                GameConstants.difficulty = 3;
                level_three();
                break;

        }

    }
    public void nextPuzzle()//initially chooses a random puzzle from 0-8
    {
        System.Random rand = new System.Random();
        levelNum = rand.Next(0, 9);
        Debug.Log("Next Rand Level is: " + levelNum);
        startLevel();
    }

    private void puzzle_settings(int attempts, int future_paths, int previous_paths, int game_mode)
    {
        attempt_count = attempts;
        display_upcoming_path = future_paths;
        display_past_paths = previous_paths;
        load_game_mode = game_mode;
    }

}

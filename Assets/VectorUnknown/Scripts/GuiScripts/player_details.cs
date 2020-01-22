using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_details : MonoBehaviour {

	public Text player_position, goal_position, attempts, attemptLabel;
	public GameObject puzzle_manager;

	public void Update () {
		string[] details = puzzle_manager.GetComponent< UFO_PuzzleManager>().game_details ();
        UFO_PuzzleManager manager = puzzle_manager.GetComponent<UFO_PuzzleManager>();
		player_position.text = details [0];
		goal_position.text = details [1];
        if (manager.puzzle_info.game_mode == 0 || manager.puzzle_info.game_mode == 1)
        {
            attempts.text = details[2];
            attemptLabel.text = "Attempts";
        }
        else
        {
            GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
            attempts.text = "" + (goals.Length);
            attemptLabel.text = "Goals Remaining";
        }

    }
}

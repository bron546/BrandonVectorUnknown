using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_details : MonoBehaviour {

	public Text player_position, goal_position, attempts;
	public GameObject puzzle_manager;

	void Update () {
		string[] details = puzzle_manager.GetComponent< UFO_PuzzleManager>().game_details ();
		player_position.text = details [0];
		goal_position.text = details [1]; 
		attempts.text = details [2];
	}
}

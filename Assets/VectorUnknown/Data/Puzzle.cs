using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Puzzle")]
public class Puzzle : ScriptableObject {

	public int attempt_count; 				// limiting number of attempts. ( player can only try X amount of times. -1 represents infinite attempts)
	// trace mode
	public int display_past_paths; 			// 1 is on, path player takes will be rendered
											// 0 is off, path player takes is invisible
	// future sight
	public int display_upcoming_path; 		// 1 is on, the next possible vector path will be rendered
											// 0 is off, the next possible vector path will remain inivisible

	public override string ToString(){
		return "Attempts : " + attempt_count.ToString () +
		"\nTrace : " + (display_past_paths == 1 ? "ON" : "OFF") +
		"\nFuture Sight : " + (display_upcoming_path == 1 ? "ON" : "OFF");
	}
}

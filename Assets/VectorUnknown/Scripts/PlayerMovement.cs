using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public GameObject GameManager;
	public GameObject Formula;

	private int State = 0;
	[SerializeField]
	private Vector3 StartPosition;
	[SerializeField]
	private Vector3 EndPosition;
	[SerializeField]
	private Vector3 Direction;

	private Queue<Vector3> Route = new Queue<Vector3>();
	private int GameMode;

	// Update is called once per frame
	void Update () {

		if (State == 1) {
			if (Direction == Vector3.zero) {
				Direction = Route.Dequeue();
				StartPosition = transform.position;
				EndPosition = transform.position + Direction;
				transform.LookAt (EndPosition);
				transform.Rotate (0, -90f, 0);
			}

			transform.position += GameConstants.Speed * Time.deltaTime * Vector3.Normalize(Direction);

			if (Vector3.Distance(transform.position,StartPosition) >= Vector3.Distance(EndPosition,StartPosition)) {
				transform.position = EndPosition;
				Direction = Vector3.zero;
				if (Route.Count == 0) {
					State = 0;
                    UFO_PuzzleManager manager = GameManager.GetComponent<UFO_PuzzleManager>();
                    formula_controller controller = Formula.GetComponent<formula_controller>();
                    manager.TestSuccess (EndPosition);
					controller.reset ();
                    if (manager.puzzle_info.GetDisplayUpcomingPath() == 1)
                        controller.line_1.gameObject.SetActive(true);
				}
			}
		}

	}

	public bool is_moving(){
		//returns true if player is moving along a path
		return (State == 1);
	}

	public void Move (Vector3[] route) {
		for (int i = 0; i < route.Length; i++) {
			Route.Enqueue (route [i]);
		}
		State = 1;
	}

}

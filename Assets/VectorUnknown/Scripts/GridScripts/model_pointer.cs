using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class model_pointer : MonoBehaviour {

	private Vector3 previous_position;
	private LineRenderer grid_shadow;

	void Start () {
		grid_shadow = GetComponent< LineRenderer> ();
		grid_shadow.positionCount = 2;

		previous_position = transform.position;
		grid_shadow.SetPosition (0, grid_space (previous_position));
		grid_shadow.SetPosition (1, grid_space (previous_position));
	}

	void Update () {
		if (transform.position != previous_position) {
			previous_position = transform.position;
			grid_shadow.SetPosition (0, grid_space (previous_position));
			grid_shadow.SetPosition (1, grid_space (previous_position));
		}
	}

	private Vector3 grid_space( Vector3 world_space){
		return new Vector3 (world_space.x, .25f, world_space.z);
	}
}

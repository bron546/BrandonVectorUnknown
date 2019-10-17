using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author: Nate Cortes
 * 	Rotate camera script for IOLA vector game
 */

public class rotate_camera : MonoBehaviour
{

    [SerializeField]
    private Vector3 target;         //center of game board
    private GameObject reflect;     //Reflects the Camera's position across the gameboard; used for billboard numbers
    private GameObject follow_reflect;//Smooth reflect movement
    private GameObject follow;      //transform to smoothly follow
    private float v_lock = 0.0f;    //vertical rotation limit, will not rotate >36 degrees above the board
    private Vector3 velocity, reflect_velocity;//camera follow velocity
    private bool keypress = false;

    void Start()
    {
        velocity = reflect_velocity = Vector3.zero;
        /* Grab all objects for camera positions */
        target = GameObject.FindGameObjectWithTag("Center").transform.position;
        follow = GameObject.FindGameObjectWithTag("Follow");
        reflect = GameObject.FindGameObjectWithTag("Reflect");
        follow_reflect = GameObject.FindGameObjectWithTag("FollowReflect");
        /* Set the positions relative to the center of the board */
        transform.RotateAround(target, transform.TransformDirection(Vector3.right), 45.0f);
        follow.transform.position = transform.position;
        follow_reflect.transform.position = transform.position;
        follow_reflect.transform.position = new Vector3(-transform.position.x, -transform.position.y, -transform.position.z);
        reflect.transform.position = follow_reflect.transform.position;
        /* Set all objects to look at the center of the board */
        transform.LookAt(target);
        follow.transform.LookAt(target);
        reflect.transform.LookAt(target);
        follow_reflect.transform.LookAt(target);
    }

    void Update()
    {
        //horizontal rotations
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !keypress)
        {
            follow.transform.RotateAround(target, Vector3.up, 45.0f);
            follow_reflect.transform.RotateAround(target, Vector3.up, 45.0f);
            keypress = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !keypress)
        {
            follow.transform.RotateAround(target, Vector3.down, 45.0f);
            follow_reflect.transform.RotateAround(target, Vector3.down, 45.0f);
            keypress = true;
        }
        //vetrical rotations
        if (Input.GetKeyDown(KeyCode.UpArrow) && v_lock < 30.0f && !keypress)
        {
            spin_vertical(12.0f, 1);
            keypress = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && v_lock > -30.0f && !keypress)
        {
            spin_vertical(12.0f, -1);
            keypress = true;
        }
        //Reset
        if (Input.GetKeyDown(KeyCode.R) || v_lock > 36.0f || v_lock < -36.0f)
            reset_camera();

        //SmoothDamp to move the camera to the follow position, then retarget the center of the board
        transform.position = Vector3.SmoothDamp(transform.position, follow.transform.position, ref velocity, 0.3f);
        transform.LookAt(target);

        reflect.transform.position = Vector3.SmoothDamp(reflect.transform.position, follow_reflect.transform.position, ref reflect_velocity, 0.3f);
        reflect.transform.LookAt(target);
        keypress = false;
    }

    void spin_vertical(float degree, int direction)
    {
        if (direction > 0)
        { // if positive, rotate up
            follow.transform.RotateAround(target, transform.TransformDirection(Vector3.right), degree);
            follow_reflect.transform.RotateAround(target, transform.TransformDirection(Vector3.right), degree);
            v_lock += degree;
        }
        else
        { // if negative, rotate down
            follow.transform.RotateAround(target, transform.TransformDirection(Vector3.left), degree);
            follow_reflect.transform.RotateAround(target, transform.TransformDirection(Vector3.left), degree);
            v_lock -= degree;
        }
    }

    void reset_camera()
    {
        follow.transform.position = new Vector3(0f, 36.06245f, -34.64823f);
        follow_reflect.transform.position = new Vector3(0f, -36.06245f, 34.64823f);
        v_lock = 0f;
    }
}

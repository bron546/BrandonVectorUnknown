using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal_collide : MonoBehaviour
{
    public UFO_PuzzleManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UFO_PuzzleManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        if (manager.puzzle_info.game_mode == 2 || manager.puzzle_info.game_mode == 3)
        {
            //if(manager.puzzle_info.game_mode == 3)
            //{
            //        this.GetComponent<MeshRenderer>().enabled = true;
            //        float i = 2.0f;
            //        while(i > 0)//wait for two seconds so that player sees the object that was hit
            //        {
            //            i -= Time.deltaTime;
            //        }
            //}
            manager.decrement_goals();
            //Debug.Log( manager.number_of_keys);
            GameObject.Destroy(gameObject);
        }
    }
}

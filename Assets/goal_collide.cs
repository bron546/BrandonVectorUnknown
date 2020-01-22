using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal_collide : MonoBehaviour
{
    public UFO_PuzzleManager manager;
    public float visTimer = 0.0f;
    public bool timerActive = false;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UFO_PuzzleManager>();
    }

    private void Update()
    {
        if(visTimer > 0 && timerActive)
        {
            visTimer -= Time.deltaTime;
        }
        else if(visTimer <= 0 && timerActive)
        {
            timerActive = false;
            //manager.decrement_goals();
            GameObject.Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !timerActive)
        if (manager.puzzle_info.game_mode == 2 || manager.puzzle_info.game_mode == 3 || manager.puzzle_info.game_mode == 4 || manager.puzzle_info.game_mode == 5)
        {
                if (manager.puzzle_info.game_mode == 4 || manager.puzzle_info.game_mode == 5)
                {
                    manager.setBasketVisible(gameObject);
                    manager.decrement_goals();
                    visTimer = 2.0f;
                    timerActive = true;
                }
                else
                {
                    manager.decrement_goals();
                    GameObject.Destroy(gameObject);
                }
        }
    }
}

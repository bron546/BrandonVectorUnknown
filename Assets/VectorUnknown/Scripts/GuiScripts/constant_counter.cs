using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Author : Nate Cortes
 * Holds a counter for a constant value. Made for CPI441 capstone. 
 */

public class constant_counter : MonoBehaviour
{
    [SerializeField]
    public int constant = 0;        // constant value, starts at 1
    private bool change = false;    //value changed y/n
    private GameObject text_display;//UI text element
    public Transform choice_dropper;//watches dropper element for vector choice
    public Transform other_choice;  //holds the other choice vector
    public Transform other_counter; //holds the other constant value 
    public GameObject player;       //reference to player to obtain player position

    public Text log;
    private static bool displayedBoundsError = false;

    void Awake()
    {

        text_display = transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");

        text_display.GetComponent<Text>().text = constant.ToString();
        GameObject temp = transform.GetChild(1).gameObject;
        temp.GetComponent<Button>().onClick.AddListener(decrement);
        temp = transform.GetChild(2).gameObject;
        temp.GetComponent<Button>().onClick.AddListener(increment);
    }

    void Update()
    {
        if (change)
            text_display.GetComponent<Text>().text = constant.ToString();

        change = false;
    }

    //Returns false if the vector line would reach beyond the grid
    public bool test_grid_boundries(int next_const)
    {
        if (GameConstants.locked)
        {
            if (choice_dropper.childCount == 1)
            {
                Vector2 location = choice_dropper.GetChild(0).GetComponent<choice_holder>().choice;
                Vector2 other = (other_choice.childCount == 1 ?
                    other_choice.transform.GetChild(0).GetComponent<choice_holder>().choice :
                    Vector2.zero
                );
                int other_constant = other_counter.GetComponent<constant_counter>().constant;

                if ((player.transform.position.x + location.x * next_const + other.x * other_constant) > 20)
                    return false;
                if ((player.transform.position.z + location.y * next_const + other.y * other_constant) > 20)
                    return false;
                if ((player.transform.position.x + location.x * next_const + other.x * other_constant) < -20)
                    return false;
                if ((player.transform.position.z + location.y * next_const + other.y * other_constant) < -20)
                    return false;

                return true;
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    public void increment()
    {
        bool test = test_grid_boundries(constant + 1);

        if (test)
        {
            constant = constant + 1;
            change = true;
        }
        else if(!displayedBoundsError)
        {
            displayBoundsError();
        }
    }

    public void decrement()
    {
        bool test = test_grid_boundries(constant - 1);

        if (test)
        {
            constant = constant - 1;
            change = true;
        }
        else if (!displayedBoundsError)
        {
            displayBoundsError();
        }
    }

    public void testStillFit()
    {
        if(!test_grid_boundries(constant))
        {
            reset();
        }
    }

    public void displayBoundsError()
    {
        log.text += "Error! Using that scalar would push you off the grid. Try another instead.\n";
        displayedBoundsError = true;
    }

    public void reset()
    {
        constant = 0;
        change = true;
    }

    public void set_zero()
    {
        constant = 0;
        change = true;
    }
}

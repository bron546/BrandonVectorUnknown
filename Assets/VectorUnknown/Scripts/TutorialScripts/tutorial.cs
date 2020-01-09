using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class tutorial : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playable_director;
    [SerializeField]
    private List< TimelineAsset> timelines = new List< TimelineAsset>();
    [SerializeField]
    private List<TimelineAsset> reverse_timelines = new List<TimelineAsset>();
    public TextMeshProUGUI instruction, title;
    private string[] tutorial_messages = new string[9]
    {
        "\tFirst things first, the goal of the game is to get the bunny on the grid to move to the goal.This tutorial will tell you how to work with the formula bar to accomplish that.\n\tUse the arrow keys to move the camera around for a different perspective of the grid. Press the 'r' key to reset the camera.",
        "\tThe minimap in the top left corner shows your position on the grid from a top down perspective.",
        "\tThese are the vector choices you have available to you during the level. This panel displays the vectors you have available to you for solving the current puzzle.",
        "\tThis is the player info panel. This shows your current position, the goal's position, and the number of attempts you have left.",
        "\tThe log panel will track your progress through the level. It contains a history of each step you take. \n\tEvery time you click \"GO\" the formula you constructed is logged to this panel.",
        "\tThis is the formula bar, where you will drag the choice vectors in order to move the bunny on the grid. \n\tClick the plus and minus buttons next to the vector to change the constant value when a vector is in the formula bar.",
        "\tThe timer keeps track of how long you've been playing the current level. \n\n\tYou can click on the timer to make it disapear and reappear.",
        "\tThese are the options buttons. Click the menu icon in the bottom right corner to expand the menu. \n\n\tIn game you will be able to restart the current level, return to level select or pull up the instructions from here.",
        "\tLet's complete this level by draging vector choices to the formula bar and setting the constant values to match the destination output to the goal position. Once you've constructed a formula to match the destination click the green GO button."
    };
    private string[] tutorial_titles = new string[9]
    {
        "Welcome!",
        "1. Minimap",
        "2. Choice Vectors",
        "3. Player Info",
        "4. Log",
        "5. Formula Bar",
        "6. Timer",
        "7. Options",
        "Finish the Puzzle"
    };

    private int current_message = 0;

    private void Start()
    {
        set_msg();
        playable_director.Play(timelines[current_message]);
    }

    private void set_msg()
    {
        instruction.text = tutorial_messages[current_message];
        title.text = tutorial_titles[current_message];
    }

    public void next_message()
    {
        if (current_message >= tutorial_messages.Length - 1 )
            return;

        current_message++;
        set_msg();

        playable_director.Play(timelines[current_message]);
    }

    public void prev_message()
    {
        if (current_message <= 0)
            return;

        current_message--;
        set_msg();

        playable_director.Play(reverse_timelines[current_message]);
    }
}

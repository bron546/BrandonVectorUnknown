using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DynamicLevelManager : MonoBehaviour
{
    /* 6/5/2020
    * Brandon Bayles
    * This script is an adaption of the hard-coded UFO_PuzzleManager level generator that is designed for easier modification of specific level details for auto-generation of levels. The code is built off of and adapted from code created for the original game. 
    * The code will ultimately be integrated into an adaptive difficulty bayesian net algorithm for usage in a thesis project. The game will need to be able to adapt details of levels on the fly, so UFO_PuzzleManager's hard-coded difficulty settings will be 
    * unsuitable for this end goal. This script will address that issue. This script will provide a series of global variables accessible to other scripts to adapt level details such as the number of keys and presence of the predictive path so that other scripts 
    * can create any combination of level intricacies instead of being restricted to specific combinations associated with specific level numbers and difficulties. 
    */
    static public DynamicLevelManager instance;
    void Awake() { instance = this; }

    public GameObject Player;
    public GameObject Goal;
    public Vector3 GoalPosition;
    public Vector2[] Choices = new Vector2[4];//Container for Vectors in the form used for UI Text
    public Vector2 Solution;
    public int number_of_attempts;
    public int number_of_keys;

    public System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
    public GameObject InfoController;
    public GameObject choice_panel;

    public GameObject key;      //key for game_mode 1
    public GameObject key_sack; //parent for all key objects
    public puzzle_info puzzle_info;
    public scorekeeper scorekeep; //generates and logs the score
    public int number_of_goals;
    public int max_key_retrys = 15;
    public int key_retrys = 0;
    public int offsetMax;
    int upperLimit = GameConstants.BaseVectors.Length;            // Num[i]= Random index number of GameConstants.BaseVectors array
    int lowerLimit = 0;
    public int spacingMax;
    public int spacingMin;
    public bool levelFinished = false;
    public bool resetFlag = false;
    public Vector2 solutionVec;
    public Vector2 offsetVec;
    public bool solutionDisplaySet = false;
    public string solutionFormat = "";

    void Start()
    {
        key_sack = GameObject.FindGameObjectWithTag("Keys");
        choice_panel = GameObject.FindGameObjectWithTag("Choices");
        puzzle_info = GetComponent<puzzle_info>();

        if (puzzle_info.tutorial == false)
        {
            scorekeep = GameObject.FindGameObjectWithTag("Score").GetComponent<scorekeeper>();
            Solution = new Vector2(0, 0);
            solutionFormat = "";
            solutionDisplaySet = false;
        }
    }

    public void NextPuzzle()
    {
        solutionFormat = "";
        solutionDisplaySet = false;
        if (puzzle_info.tutorial == false)
        {
            Solution = new Vector2(0, 0);
        }
        levelFinished = false;
        
        int[] Num = new int[2]; //Container for the random index numbers of the GameConstants.BaseVectors array
        Debug.Log(GameConstants.difficulty);
        // Randomly chooses two vectors from the GameConstants.BaseVectors array and stores them in the Choices array.
        
        if (GameConstants.difficulty == 1)
        {   //easy mode
            //choose vectors from <1,0> to <3,1> in BaseVecctors
            lowerLimit = 2;
            upperLimit = 7;
            
        }
        if (GameConstants.difficulty == 2)
        {   //medium
            //choose vectors from <1,0> to <5,1> in BaseVecctors
            lowerLimit = 2;
            upperLimit = 11;
            
        }
        if (GameConstants.difficulty == 3)
        {   //hard
            lowerLimit = 7;
            upperLimit = 16;
           
        }

        Num[0] = rnd.Next(lowerLimit, upperLimit - 1);    // Num[i] is used to select random vector
        if (Num[0] <= 1)
        {                      // If Num[0] --> <0,1> or <1,0>, then Num[1] should not be a duplicate of Num[0]
            Num[1] = rnd.Next(lowerLimit, upperLimit - 1);     // So Num[1] should be selected from among l-1 index numbers
            if (Num[1] >= Num[0])
            {
                Num[1]++; // Adjust Num[1] depending on the value of Num[0], so that Num[0]!=Num[1] and 0<=Num[1]<=l
            }
        }
        else
            Num[1] = rnd.Next(lowerLimit, upperLimit - 1);         // If Num[0] !-> <0,1> or <1,0>, then Num[1] can be equal to Num[2]

        if (GameConstants.difficulty == 1)//easy mode vector choices will always contain <1,0>
            Choices[0] = rnd.Next(0, 2) == 1 ? new Vector2(0, 1) : new Vector2(1, 0); // Choices[0]= First Random Vector (Will 	Eventually Become First Solution Vector)
        else
            Choices[0] = GameConstants.BaseVectors[Num[0]];

        Choices[1] = GameConstants.BaseVectors[Num[1]]; // Choiecs[1]= Second Random Vector (Will Eventually Become Second Solution Vector)

        // Randomly changes the directions of the vectors in the Choices array, so that they point to different quadrants.
        int quad_1 = rnd.Next(1, 5); // Quad[0]= Random quadrant for the first solution vector, can be any quadrant 1-4
        int quad_2 = rnd.Next(1, 3); // Quad[1]= Random quadrant for the second solution vector, must be adjacent to Quad[0]
        enforce_quadrants(quad_1, quad_2);

       
        // Calculate Solution and Goal Positions
        Solution = generate_solution(Num);
        GoalPosition = new Vector3(Solution.x, GameConstants.Height / GameConstants.GridSpacing, Solution.y) * GameConstants.GridSpacing;


        //shuffle choices for random display in UI
        shuffle(Choices);
        string log_path = Application.dataPath + "/puzzle_manager_logfile.txt";
        log(log_path);

        Goal.transform.position = new Vector3(Solution.x, GameConstants.Height, Solution.y);

        update_choices();

        /* Limited Tour Game Mode */
        //generate a number of keys to be collected
        if (puzzle_info.game_mode == 1)
        {
            number_of_keys = GameConstants.difficulty;
            //Debug.Log ("Keys: " + number_of_keys.ToString());
            generate_keys();
        }
        /**************************/

        Psychometrics.logEvent("Cs" + Choices[0] + Choices[1] + Choices[2] + Choices[3]);

    }

    public Vector2 generate_solution(int[] Num)
    {
        int[] Mul = new int[2]; //Container for the random constant that extends the length of the vectors
                                // Adds two new vectors to the Choices array that are proportional to the first two vectors
                                // i.e. Choices[0] and Choices[2] are linearly dependent 
                                //      Choices[1] and Choices[3] are linearly dependent 
        for (int i = 0; i < 2; i++)
        {
            if (Num[i] <= 2)
            {              // If the vector is <1,1>, <1,0>, or <0,1>, then the Mul[i] can be from 2-10, 
                Mul[i] = rnd.Next(2, 11);  // Mul[i] = constant that will determine goal placement -- Max: <10,10>, <10,0>, <0,10>
                if (Mul[i] == 6)
                {          // To keep the vectors "nice", only certain constants can multiply Choices
                    int n = rnd.Next(2, 5); // If Mul[i]=6, Choices can be multiplied by 2,3,6 (Since 6 is divisible by 2,3,6)
                    if (n == 4) n = 6;       // 4 Maps to 6
                    Choices[i + 2] = Choices[i] * n; // Add vector that is linearly dependent with Choices[i]
                }
                else if (Mul[i] == 10)
                {    // If Mul[i]=10, Choices can be multiplied by 2,3,10
                    int n = rnd.Next(2, 5); // Since 10 is divisible by 2,3,10
                    if (n == 3) n = 5;       // 3 maps to 5
                    if (n == 4) n = 10;      // 4 maps to 10
                    Choices[i + 2] = Choices[i] * n; // Add vector that is linearly dependent with Choices[i]
                }
                else if (Mul[i] == 4) Choices[i + 2] = Choices[i] * Mathf.Pow(2, rnd.Next(1, 3)); //4 is divisible by 2 and 4
                else if (Mul[i] == 8) Choices[i + 2] = Choices[i] * Mathf.Pow(2, rnd.Next(1, 4)); //8 is divisible by 2, 4, and 8
                else if (Mul[i] == 9) Choices[i + 2] = Choices[i] * Mathf.Pow(3, rnd.Next(1, 3)); //9 is divisible by 3 and 9
                else Choices[i + 2] = Choices[i] * Mul[i]; // All other possible numbers are only divisible by themselves
            }
            else if (Num[i] <= 4)
            {       // If the vector is <1,2> or <2,1>
                Mul[i] = rnd.Next(2, 6); // Then Mul[i] can be 2-5, i.e. <5,10> and <10,5> are max
                if (Mul[i] == 4) Choices[i + 2] = Choices[i] * Mathf.Pow(2, rnd.Next(1, 3)); //4 is divisible by 2 and 4
                else Choices[i + 2] = Choices[i] * Mul[i]; // Add vector that is linearly dependent with Choices[i]
            }
            else
            {                          // If the vector is <1,3>, <3,1>, <2,3>, or <3,2>
                Mul[i] = rnd.Next(2, 4); // Then Mul[i] can be 2-3, i.e. <3,9>, <9,3>, <6,9>, and <9,6>
                Choices[i + 2] = Choices[i] * Mul[i]; // Add vector that is linearly dependent with Choices[i]
            }
            int s = rnd.Next(0, 2);           //Randomly change the sign of the new vectors
            if (s == 0) Choices[i + 2] *= -1; //0 --> sign change, 1 --> no sign change
        }


        Psychometrics.logEvent("A:" + Mul[0] + "*" + Choices[0] + "+" + Mul[1] + "*" + Choices[1]);
        Vector2 test_vec = Mul[0] * Choices[0] + Mul[1] * Choices[1];


        if (test_vec.x > 20 || test_vec.x < -20 || test_vec.y > 20 || test_vec.y < -20 || test_vec == Vector2.zero) //make sure goal is within boundaries and is not at origin, otherwise, regenerate
        {
            return generate_solution(Num);

        }

        return test_vec;
    }

    public void shuffle(Vector2[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Vector2 temp = arr[i];
            int r_index = UnityEngine.Random.Range(0, arr.Length);
            arr[i] = arr[r_index];
            arr[r_index] = temp;
        }
    }

    public void enforce_quadrants(int quad_1, int quad_2)
    {
        if (quad_1 == 1 || quad_1 == 3) quad_2 *= 2; // Quad[0] = 1 or 3  -->  Quad[1] = 2 or 4
        else quad_2 = 2 * quad_2 - 1;                // Quad[0] = 2 or 4  -->  Quad[1] = 1 or 3          // All vectors initially point to quadrant 1

        if (quad_1 == 2) Choices[0].x *= -1; // Change the sign of the x component, vector now points to quadrant 2
        if (quad_1 == 3) Choices[0] *= -1;   // Change the sign of the both components, vector now points to quadrant 3
        if (quad_1 == 4) Choices[0].y *= -1; // Change the sign of the y component, vector now points to quadrant 4

        if (quad_2 == 2) Choices[1].x *= -1; // Change the sign of the x component, vector now points to quadrant 2
        if (quad_2 == 3) Choices[1] *= -1;   // Change the sign of the both components, vector now points to quadrant 3
        if (quad_2 == 4) Choices[1].y *= -1; // Change the sign of the y component, vector now points to quadrant 4
    }

    public void log(string path)
    {
        #if UNITY_EDITOR
        StreamWriter log = new StreamWriter(path);
        log.Write("");
        log.Close();

        log = new StreamWriter(path, true);

        log.WriteLine("grid spacing: " + GameConstants.GridSpacing.ToString());
        log.WriteLine("height: " + GameConstants.Height.ToString());
        log.WriteLine("player object name: " + Player.name);
        log.WriteLine("\t" + Player.transform.position.ToString());
        log.WriteLine("Goal object name: " + Goal.name);
        log.WriteLine("\t" + Goal.transform.position.ToString());
        log.WriteLine("Choices:");
        foreach (Vector2 choice in Choices)
            log.WriteLine("\t" + choice.ToString());
        log.WriteLine("Solution: " + Solution.ToString());
        log.WriteLine("Goal Positions:");
        log.WriteLine("\t" + GoalPosition.ToString());
        log.Close();
        #endif
    }

    private void generate_keys()
    {
        Vector3[] key_locations = new Vector3[number_of_keys];

        Vector2 first_part, second_part;
        for (int i = 0; i < number_of_keys; i++)
        {

            /* Step 1: Select two vectors */
            first_part = Choices[rnd.Next(0, 4)];   //select two choice vectors to create key locations
            second_part = Choices[rnd.Next(0, 4)];  //construciting locations from choice vectors ensures that the keys are reachable
                                                    /******************************/

            /* Step 2: Boundary test vectors */
            int first_min = Mathf.Min(                   //Determines the max constant a vector can be multiplied 
                Mathf.FloorToInt(5f * (4 - GameConstants.difficulty) / first_part.x),  //while remianing within the grid ( 40x40 grid, origin (0,0))
                Mathf.FloorToInt(5f * (4 - GameConstants.difficulty) / first_part.y)
            );
            int second_min = Mathf.Min(
                Mathf.FloorToInt(5f * (4 - GameConstants.difficulty) / second_part.x),
                Mathf.FloorToInt(5f * (4 - GameConstants.difficulty) / second_part.y)
            );

            if (first_min == -2147483648) { first_min = 1; } //Prevent overflow error
            if (second_min == -2147483648) { second_min = 1; }

            first_min = (first_min < 0) ? (0 - first_min) : first_min;  //sets the absolute value
            second_min = (second_min < 0) ? (0 - second_min) : second_min;
            /*********************************/

            /* Step 3: Construct location */
            Vector2 construct_location = constructLocation(first_min, first_part, second_min, second_part, key_locations);
            //construct_location *= 0.5f;
            //construct_location = remain_within_bounds(construct_location);
            // key_locations[i] = new Vector3(construct_location.x, 1f, construct_location.y);
            //Debug.Log ("Key locations: " +key_locations [i].ToString ());
            /*******************************/

            /* Step 4: Load Key at location */
            //if (!checkNear(key_locations[i].x, key_locations[i].z)) // Verify key can't spawn near origin
            if (construct_location != Vector2.zero)
            {
                key_locations[i] = new Vector3(construct_location.x, 1f, construct_location.y);
                GameObject load_key = Instantiate(key, key_locations[i], Quaternion.identity, key_sack.transform);
                Psychometrics.logEvent("K" + (i + 1) + "(" + key_locations[i].x + "," + key_locations[i].y + ")");
            }
            else
            {
                return;
            }
            /********************************/

        }
        resetFlag = false;
    }

    private Vector2 constructLocation(int first_min, Vector2 first_part, int second_min, Vector2 second_part, Vector3[] key_locations)//check to make sure valid key location within bounds, seperate from goal, and unique from other key locations was generated. Otherwise, regenerate. 
    {
        Vector2 returnVal = (rnd.Next(-first_min, first_min) * first_part) + (rnd.Next(-second_min, second_min) * second_part);
        if (key_retrys >= max_key_retrys)
        {
            key_retrys = 0;
            Player.transform.position = new Vector3(0, GameConstants.Height, 0); //Initialize Player Position
            number_of_attempts = 0;
            Solution = new Vector2(0, 0);
            Goal.transform.position = new Vector3(0, GameConstants.Height, 0);//GoalPosition
            if (Goal.transform.childCount > 1)
                Goal.transform.GetChild(1).gameObject.SetActive(false);//Reset the goal to bowl of kibble

            if (puzzle_info.game_mode == 1)
            {//Limited tour level needs to activate the lock and reset the keys
                Goal.transform.GetChild(1).gameObject.SetActive(true);
                for (int i = 0; i < key_sack.transform.childCount; i++)
                {
                    GameObject.Destroy(key_sack.transform.GetChild(i).gameObject);
                }
                key_locations = null;
            }

            NextPuzzle();
            resetFlag = true;
            return Vector2.zero;
        }
        if (returnVal.x > 20 || returnVal.x < -20 || returnVal.y > 20 || returnVal.y < -20 || (returnVal.x == Solution.x && returnVal.y == Solution.y) || (returnVal.x == 0 && returnVal.y == 0))
        {
            key_retrys++;
            returnVal = constructLocation(first_min, first_part, second_min, second_part, key_locations);
            key_retrys = 0;
            return returnVal;
        }
        if (key_locations != null)
            foreach (Vector3 keyLocation in key_locations)
            {
                if (returnVal.x == keyLocation.x && returnVal.y == keyLocation.z)
                {
                    key_retrys++;
                    returnVal = constructLocation(first_min, first_part, second_min, second_part, key_locations);
                    break;
                }
            }
        key_retrys = 0;
        return returnVal;
    }

    public void update_choices()
    {
        for (int i = 0; i < 4; i++)
        {// Choice Panel -> Droppers -> ith child of Droppers, Dropper -> data
            choice_panel.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<choice_holder>().update_choice(Choices[i]);
        }
    }

}

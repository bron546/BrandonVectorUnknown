using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
/*
 * scorekeeper.cs
 * Author: Nate Cortes
 * 
 * 'scorekeeper.cs' generates, ranks, and logs score given for completing a level. 
 *  
 * The score value is logged as a JSON object to player prefs.
 *  
 * The score function takes a value representing seconds and generates a score bounded by constants k and j.
 *      score = [ j / ( X ^ ( 1 / g)*ln( X))] + k
 * 'score' will be a value greater than or equal to k and less than or equal to j + k, ( k <= score <= j + k), 
 * not withstanding any additional bonus points added during the game.
 * 'score' gradually decays to an asymptote as the x value increases. The implementation is set to reach the asympote at 480 seconds, or 8 minutes play time. 
 * Once the player plays for more than 8 minutes they will recieve the minimum score value and no less.
 */ 
public class scorekeeper : MonoBehaviour
{

    public Text time_display;
    public puzzle_info info;
    public UFO_PuzzleManager manager;
    private float game_time;
    private bool active = true;
    private static int numAttempts;
    private static int maxAttempts;

    public class score_unit
    {   //score_unit calculates the score 
        public int _score;
        public float _time;
        [Range( 1, 3)]//one, two or three stars
        public int _stars;
        private int k = 500, j = 1000; //minimum score value is k, maximum is k + j
        private float g = 35.0f;// g horizonatlly stretches the function range

        public score_unit( int time)
        {
            _time = time > 0 ? time : 1;
            _score = gen_score(_time);
            _stars = gen_stars( _score);
        }

        private int gen_score( float seconds)
        {   // j = 1000, k = 500, g = horizontal stretch
            //int score = (int)Mathf.Ceil((this.j / Mathf.Pow(((0.75f) * (1 - (numAttempts / maxAttempts)) + (0.25f) * seconds), (1 / this.g) * Mathf.Log(((0.75f) * (1 - (numAttempts / maxAttempts)) + (0.25f) * seconds)))) + this.k);
            
            float secondScore = 1 - ((seconds - 5) / 300);//grace period of 5 seconds before score begins being deducted.
            secondScore = secondScore * 0.25f;
            int watchAttempts = numAttempts;
            float decimalAttempts = (float)numAttempts / 10;
            if (decimalAttempts == 0.1f) decimalAttempts = 0;//if done in one attempt give full attempt score
            float watchAttemptScoreBeforeScale = decimalAttempts;
            float attemptScore = 1 - decimalAttempts;
            if(attemptScore < 0)
            {
                attemptScore = 0.1f;
            }
            attemptScore = attemptScore * 0.75f;
            float combinedScore = secondScore + attemptScore;
            int score = Mathf.CeilToInt(1000 * combinedScore);
            Debug.Log("score of: " + score + " after " + seconds + " seconds and " + numAttempts + " / " + maxAttempts + " attempts");
            return score;
        }
        
        private int gen_stars( int score)
        {
            Debug.Log("generating stars");
            int max_score = 1000;
            if( // less than ~75 seconds returns three stars
                score >= max_score - (max_score / 3)
            ){ //
                return 3;
            }
            if ( //less than 11 minutes returns two stars
                score >= max_score - ( max_score / 2.14f)
            ) {
                return 2;
            }
            //greater than 11 minutes returns one star
            return 1;
        }

        public override string ToString()
        {   //returns a JSON string capturing _score, _time, and _stars
            return JsonUtility.ToJson(this).ToString();
        }
    }

    void Start()
    {
    }

    void Update()
    {
        if (this.active && manager.puzzle_info.tutorial == false)
        {
            game_time += Time.deltaTime;
            time_display.text = game_time.ToString("0.0");
            numAttempts = manager.number_of_attempts;
            maxAttempts = info.attempt_count;
        }
    }

    public void toggle()
    {// on / off switch
        active = !active;
    }

    public void generate_score()
    {
        toggle();
        score_unit score = new score_unit((int) game_time);
        string besttime = string.Format("besttime{0}", PlayerPrefs.GetInt("CurrentLevel").ToString());
        string beststars = string.Format("beststars{0}", PlayerPrefs.GetInt("CurrentLevel").ToString());

        if( PlayerPrefs.HasKey(besttime))
        {
            int champion = PlayerPrefs.GetInt(besttime);
            int contender = (int)score._time;

            if( contender < champion)
                PlayerPrefs.SetInt(besttime, contender);
        }
        else
        {
            PlayerPrefs.SetInt(besttime, (int) score._time);
        }

        if (PlayerPrefs.HasKey(beststars))
        {
            int champion = PlayerPrefs.GetInt(beststars);
            int contender = (int)score._stars;

            if (contender > champion)
                PlayerPrefs.SetInt(beststars, contender);
        }
        else
        {
            PlayerPrefs.SetInt(beststars, (int) score._stars);
        }

        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.SetString("score", score.ToString());
    }
}

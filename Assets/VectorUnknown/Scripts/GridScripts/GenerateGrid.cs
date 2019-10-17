using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject wood_outline;
    [SerializeField]
    private GameObject dirt_tile;
    [SerializeField]
    private GameObject grass_tile;
    [SerializeField]
    private GameObject road_white_rock;
    [SerializeField]
    private GameObject spotted_grass;
    [SerializeField]
    private GameObject sand;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private GameObject ice;
    [SerializeField]
    private GameObject brown;

    public int rows = 20;
    public int columns;

    public int tileSize = 1;
    public float offset = 0.5f;

    public int quadrantSize = 10;

    public float gridHeight = 0.1f;

    public char[,] tileMapped;
    public GameObject[,] tileGrid;

    public GameObject tile;

    public Transform gridHolder;

    public TextAsset[] textInputs;

    public TextAsset textInput;

    public Camera mainCamera;

    private int theme;

    // Use this for initialization
    void Start()
    {
        columns = rows;
        offset = tileSize / 2;
        //quadrantSize = rows / 2;


        wood_outline = Resources.Load("Cubix/Optimized/Prefabs/Buildings/Floors/CB_Floor_Wood_01_Side_B_Optimized", typeof(GameObject)) as GameObject;
        dirt_tile = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Rock_01_A_Optimized", typeof(GameObject)) as GameObject;
		grass_tile = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Grass_01_E_Optimized", typeof(GameObject)) as GameObject;
		spotted_grass = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Grass_01_C_Optimized", typeof(GameObject)) as GameObject;
		road_white_rock = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Rock_01_A_Optimized", typeof(GameObject)) as GameObject;
        sand = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Sand_01_A_Optimized 1", typeof(GameObject)) as GameObject;
        water = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Water_01_A_Optimized 1", typeof(GameObject)) as GameObject;
        ice = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Snow_01_B_Optimized 1", typeof(GameObject)) as GameObject;
        brown = Resources.Load("Cubix/Optimized/Prefabs/quad_squad/CB_Ground_Brown_Optimized 1", typeof(GameObject)) as GameObject;

        textInput = textInputs[Random.Range(0, textInputs.Length - 1)];
        
        theme = Random.Range(0, 4);

        tileMapped = new char[rows, columns];
        ReadTextLevels();
        InitializeGrid();

        //mainCamera.transform.position = new Vector3(rows/2,rows,columns/2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializeGrid()
    {

        offset = (float)tileSize;
        offset = offset / 2.0f;

        GameObject road_mat;

        GameObject terrain1_mat;

        GameObject terrain2_mat;

        GameObject terrain3_mat;

        if (theme == 0) //bg grass, road white
        {
            terrain1_mat = grass_tile;
            terrain2_mat = spotted_grass;
            terrain3_mat = sand;
            road_mat = road_white_rock;
        }
        else if (theme == 1) //bg sandgrass, road white
        {
            terrain1_mat = grass_tile;
            terrain2_mat = sand;
            terrain3_mat = sand;
            road_mat = road_white_rock;
        }
        else if (theme == 2) //bg ice, road water
        {
            terrain1_mat = ice;
            terrain2_mat = sand;
            terrain3_mat = grass_tile;
            road_mat = water;
        }
        else //bg grass, road sand
        {
            terrain1_mat = grass_tile;
            terrain2_mat = spotted_grass;
            terrain3_mat = brown;
            road_mat = sand;
        }

        GameObject select = wood_outline;
        for (int i = 0; i < rows; i = i + 1)
        {
            for (int jay = 0; jay < columns; jay = jay + 1)
            {
                int choice = choose_color(tileMapped[i, jay]);
                if (choice == 0)
                    select = wood_outline;
                if (choice == 1)
                    select = terrain1_mat;
                if (choice == 2)
                    select = terrain2_mat;
                if (choice == 3)
                    select = terrain1_mat;
                if (choice == 4)
                    select = terrain1_mat;
                if (choice == 5)
                    select = road_mat;
                if (choice == 6)
                    select = terrain1_mat;
                if (choice == 7)
                    select = terrain1_mat;
                if (choice == 8)
                    select = terrain3_mat;
                if (choice == 9)
                    select = terrain1_mat;
                if (choice == 10)
                    select = terrain1_mat;
                if (choice == 11)
                    select = terrain1_mat;

                float float_i = (float)i;
                float float_jay = (float)jay;
                GameObject block = Instantiate(select,
                    new Vector3(float_i - ((rows / 2) - 1.0f) - (offset),
                        0.0f, float_jay - ((columns / 2) - 1.0f) - (offset)),
                    Quaternion.identity);

				block.transform.SetParent (this.transform);
            }
        }

    }

    int choose_color(char tileData)
    {
        // 0 = outer wall
        // g = grass A
        // G = grass B
        // d = dirt A
        // D = dirt B
        // t = concrete tile
        // r = rock A
        // R = rock B
        // s = snow A
        // S = snow B
        // i = ice A
        // I = ice B


        // setting tile colors
        if (tileData == '0')
            return 0;
        else if (tileData == 'g')
            return 1;
        else if (tileData == 'G')
            return 2;
        else if (tileData == 'd')
            return 3;
        else if (tileData == 'D')
            return 4;
        else if (tileData == 't')
            return 5;
        else if (tileData == 'r')
            return 6;
        else if (tileData == 'R')
            return 7;
        else if (tileData == 's')
            return 8;
        else if (tileData == 'S')
            return 9;
        else if (tileData == 'i')
            return 10;
        else if (tileData == 'I')
            return 11;

        return 2;//tileData.baseColor = Color.green;
    }

    void ReadTextLevels()
    {
        string text = textInput.text;

        //Debug.Log(text);
        // remove newlines and returns
        //row = text[0];
        for (int i = text.Length - 1; i >= 0; i--)
        {
            if (text[i] == '\n' || text[i] == '\r' || text[i] == ' ')
            {
                string newText = text.Remove(i, 1);
                text = newText;
            }

        }

        // convert text into 2D array
        // must be correct size

        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                tileMapped[i, j] = text[index];
                index++;
            }
        }
			
    }
}
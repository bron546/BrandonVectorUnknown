using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gui_difficulty : MonoBehaviour
{

    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown.value = GameConstants.difficulty - 1;
    }

}

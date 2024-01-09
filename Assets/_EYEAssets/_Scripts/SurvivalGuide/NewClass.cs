using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewClass : MonoBehaviour
{    
    private bool _boolTest;

    public bool BoolTest
    {
        get
        {
            Debug.Log("Getting the Property: " + _boolTest);
            return _boolTest;
        }
        set
        {
            _boolTest = value;
            Debug.Log("Setting the Property: " + _boolTest);
        }
    }
}
    


   

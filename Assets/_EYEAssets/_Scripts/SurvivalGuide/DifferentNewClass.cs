using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentNewClass : MonoBehaviour
{   
    [SerializeField] private NewClass _newClass;
    private bool _localBool;


    private void Update()
    {
        //Accessing The Getter
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log(_newClass.BoolTest);
        
        //Accessing The Setter
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _localBool = !_localBool;

            var localBool = _localBool;
            Debug.Log("B00:" + localBool);
            _newClass.BoolTest = localBool;
        }
    }
}

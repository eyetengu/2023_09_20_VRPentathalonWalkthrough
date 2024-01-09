using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Behavior : MonoBehaviour
{
    [SerializeField] private int _cloudSpeed = 100;
    private float _step;
    
    void Update()
    {
        _step = .01f * _cloudSpeed * Time.deltaTime;
        CloudRingRotation();
    }

    private void CloudRingRotation()
    {
        transform.Rotate(0, _step, 0);
    }
}

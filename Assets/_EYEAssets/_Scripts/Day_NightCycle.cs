using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_NightCycle : MonoBehaviour
{
    [SerializeField] private int _speed = 1;
    private float _step;

    void Update()
    {
        _step = 0.1f * _speed * Time.deltaTime;        
        SunOrbit();
    }

    private void SunOrbit()
    {
        transform.Rotate(_step, 0, 0);
    }
}

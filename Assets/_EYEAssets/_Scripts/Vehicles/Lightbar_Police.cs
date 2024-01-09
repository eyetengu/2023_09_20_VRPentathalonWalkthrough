using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbar_Police : MonoBehaviour
{
    [SerializeField] private GameObject[] _redLights;
    [SerializeField] private GameObject[] _blueLights;
    [SerializeField] private Light[] _lights;
    [SerializeField] private bool _inPursuit;
    private bool _readyForBlue = true;
    private bool _readyForRed;


    void Start()
    {
        OperateLights_Pursuit();
    }

    void Update()
    {
        if (_inPursuit)
        {
            //OperateLights_Pursuit();
        }
    }

    void OperateLights_Pursuit()
    {
        LightUp_RedLights();
    }


    void LightUp_RedLights()
    {
        foreach (var red in _redLights)
        {
            red.SetActive(true);
        }
        foreach (var blue in _blueLights)
        {
            blue.SetActive(false);
        }
        _readyForBlue = true;
        _readyForRed = false;
        StartCoroutine(NextLightTimer());
    }

    void LightUp_BlueLights()
    {
        foreach (var red in _redLights)
        {
            red.SetActive(false);
        }
        foreach (var blue in _blueLights)
        {
            blue.SetActive(true);
        }
        _readyForRed = true;
        _readyForBlue = false;
        StartCoroutine(NextLightTimer());

    }

    IEnumerator NextLightTimer()
    {
        yield return new WaitForSeconds(.5f);

        if (_readyForBlue)
        {
            LightUp_BlueLights();
        }
        else if (_readyForRed)
        {
            LightUp_RedLights();
        }
    }
}

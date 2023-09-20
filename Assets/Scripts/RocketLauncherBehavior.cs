using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherBehavior : MonoBehaviour
{
    [SerializeField] GameObject _launchPoint;
    [SerializeField] GameObject _rocketPrefab;

    [ContextMenu("TestFire")]
    public void FireRocket()
    {
        Instantiate(_rocketPrefab,_launchPoint.transform.position,_launchPoint.transform.rotation);
    }
}

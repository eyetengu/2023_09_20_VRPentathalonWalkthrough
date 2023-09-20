using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerController : MonoBehaviour
{

    [SerializeField] private Transform cabPivot;

    [SerializeField] private Transform armPivot;

    [field: SerializeField]
    public float CabRotateDirection
    {
        get; set;
    }

    [SerializeField] private float cabRotateSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        cabPivot.Rotate(0,CabRotateDirection * cabRotateSpeed * Time.deltaTime,0);
    }
}

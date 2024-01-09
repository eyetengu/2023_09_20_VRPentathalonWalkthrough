using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRoadSection : MonoBehaviour
{
    [SerializeField] private ItsALongRoadAhead _mainRoadScript;

    [SerializeField] private float _roadSpeed = 7;
    [SerializeField] private float _speedMultiplier = 1;
    [SerializeField] private Vector3 _instantiationPoint_Road;
    [SerializeField] private Vector3 _instantiationPoint_Tree;
    [SerializeField] private Vector3 _instantiationPoint_Mountain;
    [SerializeField] private Vector3 _instantiationPoint_Building;
    [SerializeField] private bool _isRoad, _isMountain, _isTree, _isBuilding;
    private bool _isRandom;


    private void Start()
    {
        _mainRoadScript = GameObject.FindObjectOfType<ItsALongRoadAhead>();
        if (_mainRoadScript == null)
            Debug.Log("no road script");
        if(_instantiationPoint_Road == null)
            _instantiationPoint_Road = new Vector3(0, 0, 135);
        if (_instantiationPoint_Tree == null)
            _instantiationPoint_Tree = new Vector3(-25, 0, 135);
        if (_instantiationPoint_Mountain == null)
            _instantiationPoint_Mountain = new Vector3(0, 0, 135);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _speedMultiplier = 3;
        }
        else
            _speedMultiplier= 1;

        MoveTilePiece();
        LimitChecker();
    }

    private void MoveTilePiece()
    {
        var rateOverTime = _roadSpeed * _speedMultiplier * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, -rateOverTime));
    }

    private void LimitChecker()
    {
        if(_isRoad)
        {
            _mainRoadScript.SpawnNewTile();
            //if (transform.position.z < 0)
                //transform.position = _instantiationPoint_Road;
        }

        if(_isTree)
        {
            if (transform.position.z < 0)
                transform.position = _instantiationPoint_Tree;
        }

        if(_isMountain)
        {
            if (transform.position.z < 0)
                transform.position = _instantiationPoint_Mountain;
        }

        if(_isBuilding)
        {
            if (transform.position.z < 0)
                transform.position = _instantiationPoint_Building;
        }
    }
}

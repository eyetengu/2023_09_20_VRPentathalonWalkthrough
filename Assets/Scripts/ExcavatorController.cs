using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController : MonoBehaviour
{
    [SerializeField]
    GameObject _cab, _arm, _bucket;

    [SerializeField]
    float _driveSpeed = 2f;
    [SerializeField]
    float _trackRotSpeed = 15f;
    [SerializeField]
    float _equipRotSpeed = 15f;
    [SerializeField]
    float _armMinLimit = 0;
    [SerializeField]
    float _armMaxLimit = 45;
    [SerializeField]
    float _bucketMinLimit = 0;
    [SerializeField]
    float _bucketMaxLimit = 45;

    float _moveDirection = 0;
    float _trackRotDirection = 0;
    float _armRotDirection = 0;
    float _cabRotDirection = 0;
    float _bucketRotDirection = 0;

    private void Update()
    {
        //Move Body
        if (_moveDirection != 0)
            transform.Translate(Vector3.right * _moveDirection * _driveSpeed * Time.deltaTime);
        
        //Rotate Body
        if (_trackRotDirection != 0)
            transform.Rotate(Vector3.up, _trackRotDirection *_trackRotSpeed * Time.deltaTime);

        //Rotate/Lift Arm
        if (_armRotDirection > 0 && _arm.transform.rotation.eulerAngles.z < _armMaxLimit)
            _arm.transform.Rotate(Vector3.forward, _armRotDirection * _equipRotSpeed * Time.deltaTime);

        if (_armRotDirection < 0 && _arm.transform.rotation.eulerAngles.z > _armMinLimit)
            _arm.transform.Rotate(Vector3.forward, _armRotDirection * _equipRotSpeed * Time.deltaTime);

        if (_arm.transform.rotation.eulerAngles.z > 350)
            _arm.transform.rotation = Quaternion.Euler(0,0,_armMinLimit+.01f);
        else if (_arm.transform.rotation.eulerAngles.z > _armMaxLimit)
            _arm.transform.rotation = Quaternion.Euler(0, 0, _armMaxLimit-.01f);

        //Rotate Cab
        if (_cabRotDirection != 0)
            _cab.transform.Rotate(Vector3.up, _cabRotDirection * _equipRotSpeed * Time.deltaTime);

        //Rotate/Lift Arm
        if (_bucketRotDirection > 0 && _bucket.transform.rotation.eulerAngles.z < _bucketMaxLimit)
            _bucket.transform.Rotate(Vector3.forward, _bucketRotDirection * _equipRotSpeed * Time.deltaTime);

        if (_bucketRotDirection < 0 && _bucket.transform.rotation.eulerAngles.z > _bucketMinLimit)
            _bucket.transform.Rotate(Vector3.forward, _bucketRotDirection * _equipRotSpeed * Time.deltaTime);

        if (_bucket.transform.rotation.eulerAngles.z > 350)
            _bucket.transform.rotation = Quaternion.Euler(0, 0, _bucketMinLimit + .01f);
        else if (_bucket.transform.rotation.eulerAngles.z > _bucketMaxLimit)
            _bucket.transform.rotation = Quaternion.Euler(0, 0, _bucketMaxLimit - .01f);
    }

    public void MoveForward(float direction)
    {
        if (direction <= -0.1 || direction >= 0.1)
            _moveDirection = direction;
        else _moveDirection = 0;
    }

    public void RotateTracks(float direction)
    {
        if (direction <= -0.1 || direction >= 0.1)
            _trackRotDirection = direction;
        else _trackRotDirection = 0;   
    }

    public void MoveArm(float direction)
    {
        if ((direction <= -0.1) || (direction >= -0.1))
            _armRotDirection = direction;
    }

    public void RotateCab(float direction)
    {
        if ((direction <= -0.1) || (direction >= -0.1))
            _cabRotDirection = direction;
    }

    public void RotateBucket(float direction)
    {
        if ((direction <= -0.1) || (direction >= -0.1))
            _bucketRotDirection = direction;
    }
}

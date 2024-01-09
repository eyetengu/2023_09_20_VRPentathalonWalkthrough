using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDriveCar : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _speedMultiplier = 1.0f; 
    private bool _canMoveLeft, _canMoveRight;


    private void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _speedMultiplier = 2;
        else _speedMultiplier= 1;

        VehicleMovement();
        VehicleBankingBehavior();
    }

    private void VehicleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontalMovement *_speed * _speedMultiplier * Time.deltaTime, 0, 0));

        var positionInSpace = transform.position;
        if (positionInSpace.x >= 5)
        {
            _canMoveRight = false;
            transform.position = new Vector3(5, 0.5f, 15);
        }
        else
            _canMoveRight = true;
        if (positionInSpace.x <= -9.5f)
        {
            _canMoveLeft = false;
            transform.position = new Vector3(-9.5f, 0.5f, 15);

        }
        else
            _canMoveLeft = true;

    }
    private void VehicleBankingBehavior()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Debug.Log("HM" + horizontalMovement);
        var rotationalTransform = transform.rotation;
        
        if (horizontalMovement > .1f && _canMoveRight)
            transform.Rotate(0, 3 * Time.deltaTime, 0);
        
        
        
        
        
        if (horizontalMovement < -.1f && _canMoveLeft)
            transform.Rotate(0, -3 * Time.deltaTime, 0);    
    
    
    
    }

}

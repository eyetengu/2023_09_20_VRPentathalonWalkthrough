using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Vehicle_Script : MonoBehaviour
{
    [SerializeField]    private GameObject _player;
    [SerializeField] private PlayerMessages _playerMessageCenter;
    [SerializeField] private XRPlayerBehaviors _xrBehaviors;
    //[SerializeField] private XRIDefaultInputActions _vrPlayerInput;
    private ActionBasedContinuousMoveProvider _playerMover;

    [SerializeField] private bool _isDriverDoor;
    [SerializeField] private Transform _driverSeat, _passengerSeat;
    [SerializeField] private Transform _driverExit, _passengerExit;
    
    private MeshRenderer _meshRenderer;
    
    private bool _aButtonPressed;
    private bool _isDriving;
    private bool _isDriver;


    [SerializeField] private string _vehicleType;
    [SerializeField] private string _baseMessage = "Press 'A' To Enter";
    private string _completePlayerMessage;


    //INITIALIZATION
    void Start()
    {
        if (_player == null)
            _player = GameObject.Find("XR Origin");
       
        if (_xrBehaviors == null)                        
            _xrBehaviors = _player.GetComponent<XRPlayerBehaviors>();
        
        //Components
        _meshRenderer = GetComponent<MeshRenderer>();
        _completePlayerMessage = _baseMessage + "\n" + _vehicleType;
        /*
        //----------------INPUTS-----------------
        _vrPlayerInput= new XRIDefaultInputActions();
        _vrPlayerInput.Enable();

        _vrPlayerInput.XRIRightHandInteraction.RotateAnchor.performed += RotateAnchor_performed;

        _vrPlayerInput.XRIRightHand.AButtonPress.performed += AButtonPress_performed;
        _vrPlayerInput.XRIRightHand.AButtonPress.canceled += AButtonPress_canceled;

        _vrPlayerInput.XRIRightHand.XButtonPress.performed += XButtonPress_performed;
        _vrPlayerInput.XRIRightHand.XButtonPress.canceled += XButtonPress_canceled;
    }

    private void RotateAnchor_performed(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }




    //INPUTS
    private void AButtonPress_performed(InputAction.CallbackContext context)
    {
        _aButtonPressed = true;
        if(_isDriving == false)
        {
            _isDriving = true;
            _isDriver = true;

            _xrBehaviors.PrepareToDrive();
            _playerMessageCenter.DisplayPlayerMessage($"You Are Driving the\n{_vehicleType}" );
        }
    }
    private void AButtonPress_canceled(InputAction.CallbackContext context)
    {
        _aButtonPressed = false;
    }
   
    private void XButtonPress_performed(InputAction.CallbackContext context)
    {
        Debug.Log("X Button Pressed");
        if(_isDriving)
        {
            _isDriving = false;
            _xrBehaviors.CancelDriving();

            _player.transform.SetParent(null);

            if(_isDriver)
            {
                _player.transform.position = _driverExit.position;
                _player.transform.rotation = _driverExit.rotation;
            }

            if(_isDriver == false)
            {
                _player.transform.position = _passengerExit.position;
                _player.transform.rotation = _passengerExit.rotation;
            }
        }
        */
    }


    //CORE FLOW
    void Update()
    {
        if(_isDriving)
        {
            //DriveVehicle();
            RotateVehicle();
        }
    }

    private void DriveVehicle()
    {
        //var forwardMovement = _vrPlayerInput.XRILeftHandInteraction.TranslateAnchor.ReadValue<Vector2>();
        //Debug.Log("Moving" + forwardMovement);
    }

    private void RotateVehicle()
    {
        //var turnValue = _vrPlayerInput.XRIRightHandInteraction.RotateAnchor.ReadValue<Vector2>();
        //Debug.Log("Turn Value: " + turnValue);
    }


    //Behaviors
    private void SendPlayerMessage()
    {
        _playerMessageCenter.DisplayPlayerMessage(_completePlayerMessage);
    }
    private void ClearPlayerMessage()
    {
        _playerMessageCenter.ClearPlayerMessage();
    }


    //TRIGGERS
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") 
        {
            var player = other.transform;
            SendPlayerMessage();

            if(_aButtonPressed && _isDriverDoor)
            {
                _isDriver = true;
                player.SetParent(_driverSeat);
                player.position = _driverSeat.position;
                player.rotation = _driverSeat.rotation;
                ClearPlayerMessage();
            }
            else if(_aButtonPressed && _isDriverDoor == false)
            {
                _isDriver= false;
                player.SetParent(_passengerSeat);
                player.position = _passengerSeat.position;
                player.rotation = _passengerSeat.rotation;
                ClearPlayerMessage();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            ClearPlayerMessage();
        }
    }
}

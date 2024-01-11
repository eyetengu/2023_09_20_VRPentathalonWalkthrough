using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    [SerializeField] private int _healthValue = 5;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //audio
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.Health = _healthValue;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;


    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if (other.tag == "Player")
                damageable.Damage(5);
            else if (other.tag == "Zombie")
                damageable.Damage(1);
        }

        //_audioSource.PlayOneShot(_audioClip);
    }
}

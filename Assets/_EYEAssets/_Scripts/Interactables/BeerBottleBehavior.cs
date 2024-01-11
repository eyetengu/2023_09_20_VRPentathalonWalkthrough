using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottleBehavior : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] AudioClip _bottleAudio;
    private Vector3 _noiseLocation;


    void Start()
    {
        _audioSource= GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player" && collision.gameObject.tag != "Zombie")
        {
            _noiseLocation = transform.position;

            _audioSource.PlayOneShot(_bottleAudio);

            var _totalZombies = GameObject.FindGameObjectsWithTag("Zombie");

            foreach(GameObject zombie in _totalZombies)
            {
                if(Vector3.Distance(transform.position, zombie.transform.position) < 5)
                {
                    var zombieBehavior = zombie.transform.GetComponent<Enemy_BasicBehavior>();

                    if (zombieBehavior != null)
                        zombieBehavior.InvestigateDisturbance(_noiseLocation);

                    else Debug.Log("Zombie Behavior is NULL");
                }
            }
        }       
    }
}

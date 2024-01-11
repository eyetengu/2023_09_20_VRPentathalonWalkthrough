using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDoor_Behaviors : MonoBehaviour
{
    //This slider door script is meant to mimic the real-world operations of sliding doors on supermarkets and other retail establishments
    //When something enters the sensor zone(trigger volume) an audio will chime, the doors will slide open, after a delay the doors will slide closed.

    [SerializeField] private Transform[] _slidingDoors;
    [SerializeField] private float _delayClose = 3;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    private bool _areDoorsOpen;

    
    private void OnTriggerEnter(Collider other)
    {
        _audioSource.PlayOneShot(_audioClip);
        if(_areDoorsOpen == false)
        {
            _areDoorsOpen = true;
            SlideDoorsOpen();
            StartCoroutine(CloseDoorTimer());
        }
    }

    void SlideDoorsOpen()
    {
        //move _slidingDoors[0] to the left
        _slidingDoors[0].Translate(new Vector3(1.2f, 0, 0));
        //move _slidingDoors[1] to the right
        _slidingDoors[1].Translate(new Vector3(-1.2f, 0, 0));

    }

    IEnumerator CloseDoorTimer()
    {
        yield return new WaitForSeconds(_delayClose);
        SlideDoorsClosed();
    }

    void SlideDoorsClosed()
    {
        //move _slidingDoors[0] to the right
        _slidingDoors[0].Translate(new Vector3(-1.2f, 0, 0));
        //move _slidingDoors[1] to the left
        _slidingDoors[1].Translate(new Vector3(1.2f, 0, 0));
        _areDoorsOpen = false;
    }
}

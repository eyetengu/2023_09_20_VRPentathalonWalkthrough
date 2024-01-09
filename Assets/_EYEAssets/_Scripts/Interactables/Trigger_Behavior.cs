using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Trigger_Behavior : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    //[SerializeField] private GameObject _policeCruiser;
    //[SerializeField] private PlayableDirector _playableDirector;

    private void Start()
    {
        //_playableDirector = GameObject.FindObjectOfType<PlayableDirector>();
        //if (_playableDirector == null)
            //Debug.Log("Director is NULL");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            _audioSource.PlayOneShot(_audioClip);
            //_policeCruiser.SetActive(true);
            //_playableDirector.enabled= true;
            //Trigger Timeline Cutscene
        }
    }
}

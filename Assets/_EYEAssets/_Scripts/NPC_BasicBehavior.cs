using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPC_BasicBehavior : MonoBehaviour
{
    //[SerializeField] private GameObject _standardCharacterModel, _zombieCharacterModel;
    [SerializeField] private Transform _target;
    [SerializeField] private Animator _animator;
    private CapsuleCollider _collider;

    [SerializeField] private float _speed;
    private bool _isInfected;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _npcClips;

    [SerializeField] private GameObject _zombiePrefab;
    private GameObject _theDarkHalf;


    void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
        _audioSource= GetComponent<AudioSource>();
        _target = GameObject.Find("XR Origin").GetComponent<Transform>() ;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _isInfected = !_isInfected;

            if (_isInfected)
            {
                JoinTheUndead();
            }
            else JoinTheLiving();
        }

        //use a distance check to determine state to be in 
        //if(distance< 8) keep back animation   play suitable audio for player
        //if(distance < 1.5f) player being bitten animation     play suitable audio for zombie and player


        //AvoidPlayer();
    }

    private void AvoidPlayer()
    {
        MoveAwayFromPlayer();
        TurnAwayFromPlayer();
    }

    //ENEMY MOVEMENT
    private void MoveAwayFromPlayer()
    {
        var step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, -step);
    }    
    private void TurnAwayFromPlayer()
    {
        Vector3 targetDirection = _target.position - transform.position;
        float singleStep = -_speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    private void JoinTheUndead()
    {
        _theDarkHalf = Instantiate(_zombiePrefab, transform.position, transform.rotation);
        Destroy(gameObject, 0.05f);
    }
    private void JoinTheLiving()
    {
        Destroy(_theDarkHalf);
        
    }
    


    private void FollowPlayer()
    {

    }



    //NPC STATUS
    private void InfectCharacter()
    {
        _collider.enabled = false;

        _animator.SetBool("Spasming", true);
        _audioSource.Stop();
        _audioSource.PlayOneShot(_npcClips[0]);

        //play animation of epilepsy or similar reactions
        //play appropriate audio
        StartCoroutine(ZombieTurningRate());
    }


    //TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Zombie")
            InfectCharacter();  
    }

    //COROUTINES
    IEnumerator ZombieTurningRate()
    {
        yield return new WaitForSeconds(5);
        
        JoinTheUndead();
    }

    
}

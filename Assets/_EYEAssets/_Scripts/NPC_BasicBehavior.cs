using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.UI.Layout;
using static UnityEngine.GraphicsBuffer;

public class NPC_BasicBehavior : MonoBehaviour
{
    //Planet Of The Zombies(like Planet Of The Apes)
    //you find yourself alone on a zombie planet. everything is as it is on earth but instead of people its zombies.
    //... everywhere.
    //Wait a minute. you recognize some of them. That's your neighbor. Are you in a hallucinogenic state? Alternate Reality? A Zombie Game?
    //Probably, but that's the least of your worries...


    //NPC Behaviors- Idle, Walk, Run, WardOff, TurnToRun, Die/TransitionToZombie
    //NPC Waypoint system
    //Distance checker for zombie- nothing, wave, wardoff/turn to run




    [SerializeField] private enum NPCState { Idle, Patrol, WardOff, Flee, Hit, Spew, Die }
    private NPCState _currentNPCState;

    [SerializeField] private Transform _enemyZombieTransform;
    [SerializeField] private Transform _playerTarget;

    //Waypoints
    [SerializeField] private Transform[] _waypoints;
    private int _waypointID;
    private Transform _currentWaypoint;


    private float _distance;
    private bool _isStunned, _isWardingOff, _isFleeing;
    
    [SerializeField] private float _speed;
    private float _step;


    //NPC
    [SerializeField] private Animator _animator;
    private CapsuleCollider _collider;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _npcClips;


    //Zombie
    private bool _isInfected, _isNPCDead;
    [SerializeField] private GameObject _zombiePrefab;
    private GameObject _theDarkHalf;


    //INITIALIZATION
    void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
        _audioSource= GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _playerTarget = GameObject.Find("XR Origin").GetComponent<Transform>() ;
        _zombiePrefab = GameObject.FindGameObjectWithTag("Zombie");
        _step = _speed * Time.deltaTime;
    }


    //CORE FLOW
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _isInfected = !_isInfected;

            if (_isInfected)
            {
                JoinTheUndead();
            }
        }
        ZombieDistanceChecker();
        NPCStateMachine();
        
    }
    private void ZombieDistanceChecker()
    {
        if(_isNPCDead == false)
        {
            var distanceToZombie = Vector3.Distance(transform.position, _enemyZombieTransform.position);
            _distance = distanceToZombie;

            if (distanceToZombie < 1)            
                _currentNPCState = NPCState.Die;
            
            else if (distanceToZombie < 2)            
                _currentNPCState = NPCState.Hit;
            
            else if(distanceToZombie < 5)            
                _currentNPCState = NPCState.Flee;
            
            else if (distanceToZombie < 7 && _isFleeing == false)
                _currentNPCState = NPCState.WardOff;
            else
                _currentNPCState = NPCState.Patrol;
        }
    }
    private void NPCStateMachine()
    {
        switch(_currentNPCState)
        {
            case NPCState.Idle:
                _animator.SetFloat("Speed", 0.0f);
                Debug.Log("Idle State");
                break;

            case NPCState.Patrol:                
                _animator.SetFloat("Speed", 0.5f);
                FollowWaypoints();
                Debug.Log("Patrol State");
                break;

            case NPCState.WardOff:  
                TurnToFaceZombie();
                _animator.SetFloat("Speed", 0.0f);
                if(_isWardingOff == false)
                {
                    _isWardingOff = true;
                    _animator.SetTrigger("WardOff");
                    StartCoroutine(WardOffTimer());
                }
                Debug.Log("WardOff State");
                break;

            case NPCState.Flee:
                EscapeZombie();
                _animator.SetFloat("Speed", 1.0f);
                if(_isFleeing == false)
                {
                    _isFleeing = true;
                    StartCoroutine(EscapeTimer());
                }
                Debug.Log("Flee State");
                break;

            case NPCState.Hit:
                if(_isStunned == false)
                {
                    _animator.SetFloat("Speed", 0.0f);
                    _audioSource.Stop();
                    _audioSource.PlayOneShot(_npcClips[0]);
                    _isStunned = true;  
                    _animator.SetTrigger("Hit");
                    StartCoroutine(NPCStunnedTimer());
                }
                Debug.Log("Hit State");
                break;

            case NPCState.Spew:
                _animator.SetTrigger("Spew");
                StartCoroutine(SpewTimer());
                break;

            case NPCState.Die:
                if(_isNPCDead == false)
                {
                    _isNPCDead = true;
                    _animator.SetTrigger("Die");
                    InfectCharacter();
                }
                Debug.Log("Die State");
                //_animator.ResetTrigger("Die");
                break;


                default: break;
        }
    }
    

    //-----NPC_STATES-----
    //PATROL- Waypoints
    private void ChooseNextWaypoint()
    {
        _waypointID++;

        if (_waypointID > _waypoints.Length - 1)
            _waypointID = 0;
    }
    private void FollowWaypoints()
    {
        _currentWaypoint = _waypoints[_waypointID];
        var step = _speed * Time.deltaTime;
        var distanceToWaypoint = Vector3.Distance(transform.position, _currentWaypoint.position);
        
        Debug.Log("Following Waypoint #" + _waypointID);

        //Turn To Face Waypoint
        Vector3 targetDirection = _waypoints[_waypointID].position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection); 

        //Move To Waypoint
        if (distanceToWaypoint > 0.5f)        
            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, step);        
        else
            ChooseNextWaypoint();
    }


    //WARD OFF- FaceYourFears
    private void TurnToFaceZombie()
    {
        Vector3 targetDirection = _enemyZombieTransform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }


    //FLEE- Escape From Zombie
    private void EscapeZombie()
    {
        FleeZombie();
        TurnAwayFromZombie();
    }
    private void FleeZombie()
    {
        var step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _enemyZombieTransform.position, -step);
    }    
    private void TurnAwayFromZombie()
    {
        Vector3 targetDirection = _enemyZombieTransform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, -_step, 0.0f);
        
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        Debug.DrawRay(transform.position, newDirection, Color.red);
    }
    
    
    //DIE- Transition Character
    private void InfectCharacter()
    {
        Debug.Log("Character Infected");
        _collider.enabled = false;

        _animator.SetBool("Spasming", true);
        _audioSource.Stop();
        _audioSource.PlayOneShot(_npcClips[0]);

        //play animation of epilepsy or similar reactions
        //play appropriate audio
        StartCoroutine(ZombieTurningRate());
    }
    private void JoinTheUndead()
    {
        _audioSource.PlayOneShot(_npcClips[0]);
        _theDarkHalf = Instantiate(_zombiePrefab, transform.position, transform.rotation);
        //Destroy(gameObject, 0.05f);
    }
  
    
    //TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Zombie")
            //InfectCharacter();  
    }

    
    //COROUTINES
    IEnumerator WardOffTimer()
    {
        yield return new WaitForSeconds(3);
        _isWardingOff = false;
        _currentNPCState = NPCState.Patrol;
    }
    
    IEnumerator EscapeTimer()
    {
        yield return new WaitForSeconds(5);
        _isFleeing= false;
        _currentNPCState = NPCState.WardOff;
    }
    IEnumerator NPCStunnedTimer()
    {
        yield return new WaitForSeconds(1.5f);
        _animator.SetTrigger("Spew");
        _currentNPCState = NPCState.Spew;
        _isStunned = false;
    }

    IEnumerator SpewTimer()
    {
        yield return new WaitForSeconds(2.0f);
        _currentNPCState = NPCState.Flee;
    }

    IEnumerator ZombieTurningRate()
    {
        yield return new WaitForSeconds(3.0f);        
        JoinTheUndead();
    }     
}

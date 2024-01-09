using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.Content.UI.Layout;
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

    [SerializeField] private float _distance;
    private bool _isStunned, _isWardingOff, _isFleeing;
    private bool _hasFleeCoroutineRun;

    [SerializeField] private float _speed;
    private int _damage;
    private float _step;


    //NPC
    [SerializeField] private Animator _animator;
    private CapsuleCollider _collider;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _npcClips;


    //Zombie
    private bool _isInfected, _hasNPCDied;
    [SerializeField] private GameObject _zombiePrefab;
    private GameObject _theDarkHalf;


    //INITIALIZATION
    void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        //_playerTarget = GameObject.Find("XR Origin").GetComponent<Transform>();
        _enemyZombieTransform = GameObject.FindGameObjectWithTag("Zombie").GetComponent<Transform>();
        _step = _speed * Time.deltaTime;
    }


    //CORE FLOW
    void FixedUpdate()
    {               
        ZombieDistanceChecker();
        
        if (_isFleeing)
            _currentNPCState = NPCState.Flee;
        
        NPCStateMachine();
    }

    private void ZombieDistanceChecker()
    {
        if(_damage >= 3)
            _hasNPCDied= true;

        if (_hasNPCDied == false)
        {
            var distanceToZombie = Vector3.Distance(transform.position, _enemyZombieTransform.position);
            _distance = distanceToZombie;

            //if (distanceToZombie < 1.5)                                  //HIT- if Zombie is < 5ft away - (3x = infection => Die)
                //_currentNPCState = NPCState.Hit;

            //else if (distanceToZombie < 5)                               //FLEE- if Zombie is < ~10ft away                                            
                //_isFleeing = true;

            if (distanceToZombie < 8 && _isFleeing == false)       //WARD OFF- if Zombie < 17 ft away AND is NOT fleeing already
                _currentNPCState = NPCState.WardOff;

            else if(distanceToZombie > 8 && _isFleeing == false)        //if Zombie is more than 17 ft away- PATROL
                _currentNPCState = NPCState.Patrol;                    
        }
        else
        {            
            _currentNPCState = NPCState.Die;                            
        }
    }
    private void NPCStateMachine()
    {
        switch (_currentNPCState)
        {
            case NPCState.Idle:
                _isWardingOff = false;
                _isFleeing = false;
                SetIdleAnim();
                    break;

            case NPCState.Patrol:
                _isWardingOff = false;
                _isFleeing = false;

                SetWalkAnim();
                FollowWaypoints();
                    break;

            case NPCState.WardOff:
                TurnToFaceZombie();
                _isFleeing = false;
                if (_isWardingOff == false)
                {
                    _isWardingOff= true;
                    WardOffZombie();
                }
                    break;

            case NPCState.Flee:
                _isWardingOff = false;
                SetRunAnim();
                //EscapeZombie();
                //if (_isFleeing)
                    //EscapeZombie();
                    break;

            case NPCState.Hit:
                _isFleeing = false;
                _isWardingOff = false;

                NPCHasBeenHit();
                    break;

            case NPCState.Spew:
                _isFleeing = false;
                _isWardingOff = false;

                _animator.SetTrigger("Spew");
                StartCoroutine(SpewTimer());
                    break;

            case NPCState.Die:
                _isFleeing = false;
                _isWardingOff = false;

                if (_hasNPCDied ==false)
                {
                    _hasNPCDied = true;
                    _animator.SetTrigger("Die");
                    InfectCharacter();
                }
                _animator.ResetTrigger("Die");
                   break;

            default: break;
        }
    }


    //-----NPC_STATE_Animations-----
    private void SetIdleAnim()
    {
        _animator.SetFloat("Speed", 0.0f);
    }
    private void SetWalkAnim()
    {
        _animator.SetFloat("Speed", 0.5f);
    }
    private void SetRunAnim()
    {
        _animator.SetFloat("Speed", 1.0f);
    }
   

    //PATROL- Waypoints
    private void ChooseNextWaypoint()
    {
        _waypointID++;

        if (_waypointID > _waypoints.Length - 1)
            _waypointID = 0;
    }
    private void FollowWaypoints()
    {
        //_animator.SetFloat("Speed", 1.0f);
        _currentWaypoint = _waypoints[_waypointID];
        var step = _speed * Time.deltaTime * 0.6f;
        var distanceToWaypoint = Vector3.Distance(transform.position, _currentWaypoint.position);

        Debug.Log("Following Waypoint #" + _waypointID);

        //Turn To Face Waypoint
        Vector3 targetDirection = _waypoints[_waypointID].position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step * 2, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        //Move To Waypoint
        if (distanceToWaypoint > 0.5f)
            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, step);
        else
            ChooseNextWaypoint();
    }

    //WARD OFF- FaceYourFears
    private void WardOffZombie()
    {
        _animator.SetTrigger("WardOff");
        SetIdleAnim();
        StartCoroutine(WardOffTimer());
    }
    private void TurnToFaceZombie()
    {
        Vector3 targetDirection = _enemyZombieTransform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    //FLEE- Escape From Zombie
    private void EscapeZombie()
    {
        Debug.Log("Escaping Zombie");
        if(_hasFleeCoroutineRun == false)
        {
            _hasFleeCoroutineRun = true;
            StartCoroutine(EscapeTimer());
        }
        SetRunAnim();
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
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, -_step *2, 0.0f);
        
        transform.rotation = Quaternion.LookRotation(newDirection);
        SetRunAnim();
        Debug.DrawRay(transform.position, newDirection, Color.red);
    }
        
    //HIT
    private void NPCHasBeenHit()
    {
        if(_isStunned == false)
        {
            _isStunned = true;

            _damage++;
            Debug.Log("Damage: " + _damage);
            if(_damage >= 3)
            {
                _currentNPCState = NPCState.Die;
                Debug.Log("NPC Should Be Dead");
            }

            _animator.SetFloat("Speed", 0.0f);
            _animator.SetTrigger("Hit");

            _audioSource.Stop();
            _audioSource.PlayOneShot(_npcClips[0]);

            StartCoroutine(NPCStunnedTimer());
        }
    }

    //DIE- Transition Character
    private void InfectCharacter()
    {
        //play animation of epilepsy or similar reactions
        //_animator.SetBool("Spasming", true);
        _collider.enabled = false;

        //play appropriate audio
        _audioSource.Stop();
        _audioSource.PlayOneShot(_npcClips[0]);

        Debug.Log("Character Infected");
        
        StartCoroutine(ZombieTurningRate());
    }
    private void JoinTheUndead()
    {
        _audioSource.PlayOneShot(_npcClips[0]);
        Instantiate(_zombiePrefab, transform.position, transform.rotation);
        Destroy(gameObject, 0.05f);
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
    }    
    IEnumerator EscapeTimer()
    {
        yield return new WaitForSeconds(2);
        SetWalkAnim();
        _currentNPCState = NPCState.WardOff;
        _isFleeing = false;
        _hasFleeCoroutineRun = false;
    }
    IEnumerator NPCStunnedTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("Spew");
        _currentNPCState = NPCState.Spew;
        _isStunned = false;
        SetRunAnim();
    }
    IEnumerator SpewTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _currentNPCState = NPCState.Flee;
        SetRunAnim();
    }
    IEnumerator ZombieTurningRate()
    {
        yield return new WaitForSeconds(1.5f); 
        SetIdleAnim();
        JoinTheUndead();
    }     
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class Enemy_BasicBehavior : MonoBehaviour
{
    [SerializeField] private enum AIState { Idle, Chase, Attack, Wander, Die, Investigate, Scream }
    private AIState _currentState;

    //Components
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Transform _target, _host;
    private CapsuleCollider _collider;
    private Animator _animator;
    [SerializeField] private Image _playerIndicator;
    [SerializeField] private Slider _zombieHealthBar;

    //Audio
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _zombieAudio;
    [SerializeField] private AudioClip[] _zombieAttackAudio;
    [SerializeField] private AudioClip[] _zombieBreathingClip;
    [SerializeField] private AudioClip[] _randomActionClip;
    [SerializeField] private StealthZombie_UI _uiManager;

    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToPlayer, _distanceToHost;
    private bool _canStrike;

    //Zombie Stats    
    private int _maxZombieHealth = 30;
    private int _zombieHealth;
    private bool _isThisEnemyDead;

    //Zombie Movement
    private bool _isFreeToWander, _isIdling;
    private bool _hasChosenDirectionToWalk;
    private float _directionToWalk;
    private float _zombieWalkSpeed;
    private float _timeToWalk;
    [SerializeField] private Vector3 _aDisturbanceInTheForce;
    private bool _isADisturbanceInTheForce;
    [SerializeField] private bool _isTurboCrawler;

    [SerializeField] private GameObject _zombieAlterEgo;

    //INITIALIZATION
    private void Start()
    {
        _uiManager = GameObject.FindObjectOfType<StealthZombie_UI>();

        _playerHealth = GameObject.Find("XR Origin").GetComponent<PlayerHealth>() ;
        
        _animator  = GetComponent<Animator>();
        _animator.ResetTrigger("Die");

        _collider= GetComponent<CapsuleCollider>();
        
        _zombieHealth = _maxZombieHealth;
        _zombieHealthBar.value = _zombieHealth;

        _speed = Random.Range(1.8f, 2.4f);
        _canStrike = true;

        if (_target == null)
            _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        else Debug.Log("Player is NULL");

    }


    //FLOW
    void Update()
    {
        if(_isThisEnemyDead == false)
        {            
            if(_isADisturbanceInTheForce)
            {
                WalkToDisturbance();
                _currentState = AIState.Investigate;
            }
            else
            {
                //PLAYER Distance Checks
                _distanceToPlayer = Vector3.Distance(transform.position, _target.position);

                if (_distanceToPlayer > 8)
                    _currentState = AIState.Wander;

                if (_distanceToPlayer < 8 && _distanceToPlayer > 2)                
                    _currentState = AIState.Chase;

                if (_isTurboCrawler && _distanceToPlayer < 2)
                {
                    _currentState = AIState.Attack;
                    if (_canStrike)
                    {
                        //_canStrike = false;
                        //SetJumpAnim();
                        //StartCoroutine(AttackCooldown());
                    }
                }


                if(_distanceToPlayer < 1)               
                    _currentState = AIState.Attack;


                /*
                //HOST Distance Checks
                var host = GameObject.FindGameObjectWithTag("NPC");
                
                _distanceToHost = Vector3.Distance(transform.position, host.transform.position);
                if (_distanceToHost < 8)
                {
                    MoveTowardsHost();
                    RotateTowardsHost();
                }
                else if (_distanceToHost < 2)
                {
                    RotateTowardsHost();
                    if (_canStrike)
                    {
                        _canStrike = false;
                        PlayRandomAttackAudio();

                        if (_isTurboCrawler)
                            SetJumpAnim();
                        else
                            SetAttackAnim();

                        

                        StartCoroutine(AttackCooldown());
                    }
                }
                else
                    _currentState = AIState.Wander;

                */

            }
            
            //Methods
            EnemyStateMachine();
            PlayZombieBreathing();

            //PlayerInput
            if(Input.GetKeyDown(KeyCode.E))
            {
                SetJumpAnim();
                RandomAction();
            }                
        }
    }

    private void EnemyStateMachine()
    {
            switch(_currentState)
            {
                case AIState.Idle:
                    Idle();
                        break; 

                case AIState.Chase:
                    SetWalkAnim();
                    MoveTowardsPlayer();
                    TurnTowardsPlayer();
                    //_playerIndicator.material.color = Color.yellow; //change image instead                
                        break;

                case AIState.Attack:
                    TurnTowardsPlayer();
                    if(_canStrike )
                    {
                        _canStrike= false;
                        PlayRandomAttackAudio();

                        if(_isTurboCrawler)                        
                            SetJumpAnim();                        
                        else                        
                            SetAttackAnim();                        

                        _playerHealth.TakeDamage(1);
                        _uiManager.SendPlayerAMessage("YUM! YUM! YUM!");
                        StartCoroutine(AttackCooldown());
                    }                
                    //_playerIndicator.material.color =Color.red;
                        break;

                case AIState.Wander:
                    Wander();
                        break;

                case AIState.Die:
                    _animator.SetTrigger("Die");
                    DieEnemyDie();
                    Debug.Log("DeadEnemy In Progress");
                        break;

                case AIState.Investigate:
                    WalkToDisturbance();
                    Debug.Log("Investigation In Progress");
                        break;

                case AIState.Scream:
                    RandomAction();
                    Debug.Log("Scream In Progress");

                        break;

                default: 
                        break;
            }    
    }


    //ANIMATIONS
    private void SetWalkAnim()
    {
        _animator.SetBool("Walk", true);
        _animator.SetBool("Run", false);
        _animator.ResetTrigger("Attack");
    }
    private void SetJumpAnim()
    {
        _animator.SetTrigger("Jump");
        //_animator.ResetTrigger("Jump");
    }
    private void SetAttackAnim()
    {
        StopMovementAnimations();
        //_animator.ResetTrigger("Attack");
        _animator.SetTrigger("Attack");
    }
    private void SetScreamAnim()
    {
        StopMovementAnimations();
        _animator.SetTrigger("Scream");
    }
    private void StopMovementAnimations()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Walk", false);
    }


    //ENEMY MOVEMENT
    private void Wander()
    {
        //move in a direction for a period of time
        //idle for a period of time
        //repeat
        if(_isFreeToWander)
        {
            //Debug.Log("FREE TO WANDER");
            if(_hasChosenDirectionToWalk == false)
            {
                _hasChosenDirectionToWalk = true;

                _directionToWalk = Random.Range(0, 360);
                _zombieWalkSpeed = 1 * _speed * Time.deltaTime;
                _timeToWalk = Random.Range(2, 5);
                transform.Rotate(0, _directionToWalk, 0);
            }
            WalkInADirection();

            StartCoroutine(WalkTimer(_timeToWalk));
        }
        else
        {
            if(_isIdling == false)
            {
                //Debug.Log("IS IDLING");
                _isIdling = true;

                var idleTime = Random.Range(.5f, 15);
                Idle();
                StartCoroutine(IdleCooldown(idleTime));
            }
        }
    }
    private void Idle()
    {
        transform.Translate(new Vector3(0, 0, 0));
        _animator.SetBool("Walk", false);
    }
    private void WalkInADirection()
    {
        transform.Translate(new Vector3(0, 0, _zombieWalkSpeed));
        SetWalkAnim();

    }
    private void MoveTowardsPlayer()
    {
        var step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }
    private void TurnTowardsPlayer()
    {
        Vector3 targetDirection = _target.position - transform.position;
        float singleStep = _speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }    
    private void RandomAction()
    {
        var randomAction = Random.Range(0, _randomActionClip.Length-1);
        _audioSource.PlayOneShot(_randomActionClip[randomAction]);
        SetScreamAnim();
    }


    //Disturbances
    private void WalkToDisturbance()
    {
        SetWalkAnim();
        var distanceToDisturbance = Vector3.Distance(transform.position, _aDisturbanceInTheForce);
        var step = _speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _aDisturbanceInTheForce, step);
        if(distanceToDisturbance> 1)
        {
            Idle();
        }

        Vector3 targetDirection = _aDisturbanceInTheForce - transform.position;
        float singleStep = _speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);



        StartCoroutine(EndInvestigationTimer());
        
        //Turn Towards
        //MoveTowards
        //_aDisturbanceInTheForce
    }
    public void InvestigateDisturbance (Vector3 noiseLocation)
    {
        Debug.Log("Investigation Begins Here");
        _isADisturbanceInTheForce= true;
        _aDisturbanceInTheForce= noiseLocation;
        
        _currentState = AIState.Investigate;
        StartCoroutine(EndInvestigationTimer());
    }


    //HEALTH
    private void TakeDamage(int damageTaken)
    {
        _canStrike = false;
        _animator.SetTrigger("Hit");
        _zombieHealth -= damageTaken;

        if(_zombieHealth < 1)
        {            
            _isThisEnemyDead = true;
            _collider.enabled = false;
            _currentState = AIState.Die;            
        }

        _zombieHealthBar.value = _zombieHealth;
        StartCoroutine(AttackCooldown());
    }
    private void DieEnemyDie()
    {
        _audioSource.PlayOneShot(_zombieAudio[4]);
        Destroy(gameObject);
    }
    private void CureZombieism()
    {
        _currentState = AIState.Die;
        _audioSource.PlayOneShot(_zombieAudio[3]);
        StartCoroutine(TimeForCureToTakeEffect());
    }
     

    //AUDIO
    private void PlayRandomAttackAudio()
    {
        var randomClip = Random.Range(0, _zombieAttackAudio.Length);

        _audioSource.PlayOneShot(_zombieAttackAudio[randomClip]);
    }
    private void PlayZombieBreathing()
    {
        if(!_audioSource.isPlaying)
        {
            var randomBreathingClip = Random.Range(0, _zombieBreathingClip.Length-1);

            _audioSource.PlayOneShot(_zombieBreathingClip[randomBreathingClip]);
        }
    }


    //TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        { 
            TakeDamage(5);
            _audioSource.PlayOneShot(_zombieAudio[3]);
        }
        if(other.tag == "Antidote")
        {
            CureZombieism();
        }
    }


    //COROUTINES
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.2f);
        _canStrike = true;
    }

    IEnumerator WalkTimer(float walkTime)
    {
        yield return new WaitForSeconds(5);
        _isFreeToWander = false;
    }

    IEnumerator IdleCooldown(float idleTime)
    {
        yield return new WaitForSeconds(5);
        _isFreeToWander= true;
        _isIdling= false;
        _hasChosenDirectionToWalk = false;
    }

    IEnumerator EndInvestigationTimer()
    {
        yield return new WaitForSeconds(3);
            
        _isADisturbanceInTheForce= false;

        if(_currentState == AIState.Investigate)
            _currentState = AIState.Wander;
    }

    IEnumerator TimeForCureToTakeEffect()
    {
        yield return new WaitForSeconds(3);
        Instantiate(_zombieAlterEgo, transform.position, transform.rotation);
        _audioSource.PlayOneShot(_zombieAudio[4]);

        Destroy(gameObject, 1);
    }
}

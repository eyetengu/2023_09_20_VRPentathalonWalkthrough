using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Animations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Idle_zombie()
    {
        _animator.SetBool("Walk", false);
    }

    public void Walk_zombie()
    {
        _animator.SetBool("Walk", true);
    }

    public void Attack_zombie()
    {
        _animator.SetTrigger("Attack");
    }

    public void Hit_zombie()
    {
        _animator.SetTrigger("Hit");
    }

    public void Die_zombie()
    {
        _animator.SetTrigger("Die");
    }

    public void Scream_Zombie()
    {
        int randomScream = Random.Range(0, 3);

        switch(randomScream)
        {
            case 0:
                _animator.SetTrigger("Scream");
                break;
            case 1:
                _animator.SetTrigger("Scream2");
                break; 
            case 2:
                _animator.SetTrigger("Scream3");
                break;

            default:
                Debug.Log("No Scream Available");
                break;
        }    
    }

}

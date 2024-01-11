using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Zombie_Health : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider _zombieHealthBar;
    private int _maxHealth = 30;
    [SerializeField] private int _health;
    private bool _isThisZombieDead;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Enemy_BasicBehavior _zombieBehavior;

    void Start()
    {
        _health = _maxHealth;
        _zombieHealthBar.value = _health;

        if (_collider == null)
            _collider = GetComponentInChildren<CapsuleCollider>();
    }

    public int Health { get; set; }

    public void Damage(int damageAmount)
    {
        _health -= 1;
        if (_health < 1)
        {
            _health = 0;
            _isThisZombieDead = true;
            _collider.enabled = false;
            _zombieBehavior.IRepeatThisZombieIsDead();
            //Zombie is dead
            //play die animation
            //destroy/disable zombie
        }
        _zombieHealthBar.value = _health;
    }    
}

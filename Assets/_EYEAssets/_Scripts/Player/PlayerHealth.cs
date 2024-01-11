using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _bloodPanel;

    private int _maxHealth = 100;
    private int _health;

    public int Health { get; set; }

    [SerializeField] private Slider _healthBar;


    void Start()
    {
        _bloodPanel.SetActive(false);

        _healthBar.maxValue= _maxHealth;
        _health = _maxHealth;   
        _healthBar.value = _health;
    }

    void Update()
    {
        var yPos = transform.position.y;
        if (yPos < -5)
            transform.position = new Vector3(0, 10, 0);
    }

    public void AddHealth(int health)
    {
        _health += health;
        _healthBar.value = _health;
    }

    public void TakeDamage(int damageReceived)
    {
        _health -= damageReceived;
        _healthBar.value = _health;
        _bloodPanel.SetActive(true);
        StartCoroutine(BloodPanelTimer());

        if(_health < 1)
        {
            PlayerIsDead();
        }
    }

    private void PlayerIsDead()
    {
        _gameManager.GameOver();
    }

    IEnumerator BloodPanelTimer()
    {
        yield return new WaitForSeconds(1.5f);
        _bloodPanel.SetActive(false);
    }

    public void Damage(int damage)
    {
        _health -= damage;
        _healthBar.value = _health;
        _bloodPanel.SetActive(true);
        StartCoroutine(BloodPanelTimer());

        if (_health < 1)
        {
            PlayerIsDead();
        }
    }
}

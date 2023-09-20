using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballHoop : MonoBehaviour
{
    Basketball _currentBall;

    public Basketball GetCurrentBall => _currentBall;

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Basketball>(out _currentBall);
    }

}

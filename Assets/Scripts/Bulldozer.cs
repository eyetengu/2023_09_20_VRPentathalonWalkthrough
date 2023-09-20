using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulldozer : MonoBehaviour
{
    [SerializeField]
    float _rotateSpeed = 15f;
    [SerializeField]
    float _turnTime = 10f;

    [ContextMenu("Activate Turn")]
    public void ActivateTurn()
    {
        StartCoroutine(TurnRoutine());
    }

    IEnumerator TurnRoutine()
    {
        float time = 0;
        while (time < _turnTime)
        {
            transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}

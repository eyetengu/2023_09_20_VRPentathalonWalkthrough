using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basketball : MonoBehaviour
{
    Vector3 _startPOS;
    Rigidbody _rigidBody;
    [SerializeField]
    float _returnTime = 2f;
    WaitForSeconds _returnTimer;
    bool _waitingForReturn;

    // Start is called before the first frame update
    void Start()
    {
        _startPOS = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _returnTimer = new WaitForSeconds(_returnTime);
    }

    public void Return()
    {
        StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        _waitingForReturn = true;
        yield return _returnTimer;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.position = _startPOS;
        _waitingForReturn = false;
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground") && !_waitingForReturn)
            Return();
        
    }
}

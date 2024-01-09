using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door_SuperSimple : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private BoxCollider _collider;


    void Start()
    {
        _meshRenderer= GetComponent<MeshRenderer>();
        _collider= GetComponent<BoxCollider>();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _meshRenderer.enabled= false;
            _collider.enabled= false;
            StartCoroutine(CloseDoor());
        }

    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(1.5f);
        _meshRenderer.enabled= true;
        _collider.enabled = true;
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox_Changer : MonoBehaviour
{
    [SerializeField] Material _skybox;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            RenderSettings.skybox = _skybox;
    }
}

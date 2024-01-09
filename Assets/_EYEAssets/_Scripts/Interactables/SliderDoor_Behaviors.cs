using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDoor_Behaviors : MonoBehaviour
{
    [SerializeField] private Transform[] _sliderDoors;



    void Start()
    {

    }

    void Update()
    {
        OpenSliderDoors();
    }

    void OpenSliderDoors()
    {
        _sliderDoors[0].Translate(new Vector3(1, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OpenSliderDoors();
            StartCoroutine(CloseSliderDoors());
        }
    }

    IEnumerator CloseSliderDoors()
    {
        yield return new WaitForSeconds(1);

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessages : MonoBehaviour
{
    [SerializeField] private GameObject _playerCanvas;
    [SerializeField] private TMP_Text _playerMessage;


    public void DisplayPlayerMessage(string incomingMessage)
    {
        _playerCanvas.SetActive(true);
        _playerMessage.text = incomingMessage;
    }
    public void ClearPlayerMessage()
    {
        _playerCanvas.SetActive(false);
        _playerMessage.text=null;
    }

    IEnumerator PlayerMessageTimer()
    {
        yield return new WaitForSeconds(3);
        ClearPlayerMessage();
    }    
}

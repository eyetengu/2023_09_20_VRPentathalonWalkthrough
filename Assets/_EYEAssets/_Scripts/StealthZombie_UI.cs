using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StealthZombie_UI : MonoBehaviour
{
    //Panels
    [SerializeField] private GameObject[] _menuPanels;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _creditsObject;
    [SerializeField] private GameObject _mainMenu;

    //PanelComponents
    [SerializeField] private GameObject _playerMessageZone;
    [SerializeField] private TMP_Text _playerMessageArea;

    //Settings
    [SerializeField] private Image _brightnessPanel;
    [SerializeField] private Slider _brightnessSlider;
    private bool _showPauseMenu;


    //INITIALIZATION
    void Start()
    {
        //Time.timeScale = 0.0f;
        InitializeZombieUI();
        StartCoroutine(PanelOffTimer());
    }
    public void InitializeZombieUI()
    {
        foreach (GameObject menu in _menuPanels)
        {
            menu.SetActive(false);
        }

        _menuPanels[0].SetActive(true);
        _buttonsPanel.SetActive(false);
    }


    //CORE FLOW
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _showPauseMenu = !_showPauseMenu;

            if (_showPauseMenu)
                DisplayMainMenu();
            else
                DisplayPlayerUI();
        }
    }


    //DISPLAY SCREENS
    public void DisplayMainMenu()
    {
        Time.timeScale = 0.0f;

        foreach (GameObject menu in _menuPanels)
        {
            menu.SetActive(false);
        }

        _menuPanels[1].SetActive(true);
        _buttonsPanel.SetActive(true);
        _exitButton.SetActive(true);
        _creditsObject.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void DisplayPlayerUI()
    {
        Time.timeScale = 1.0f;

        foreach (GameObject menu in _menuPanels)
        {
            menu.SetActive(false);
        }

        _menuPanels[2].SetActive(true);
        _buttonsPanel.SetActive(false);
    }

    public void DisplayCreditScreen()
    {
        foreach (GameObject go in _menuPanels)
        {
            go.SetActive(false);
        }
        _menuPanels[3].SetActive(true);
        _buttonsPanel.SetActive(true);
        _exitButton.SetActive(true);
        _creditsObject.SetActive(false);
        _mainMenu.SetActive(true);
    }


    //CORE METHODS
    public void SendPlayerAMessage(string incomingMessage)
    {
        _playerMessageZone.SetActive(true) ;
        _playerMessageArea.text = incomingMessage;
        StartCoroutine(ClearPlayerMessage());
    }

    //SETTINGS
    public void AdjustBrightness()
    {
        var panelColor = _brightnessPanel.color;
        panelColor.a = _brightnessSlider.value;
        _brightnessPanel.color = panelColor;
    }

    public void AdjustAudio()
    {

    }


    //COROUTINES
    IEnumerator PanelOffTimer()
    {
        yield return new WaitForSeconds(3);
        DisplayPlayerUI();
        Time.timeScale = 1.0f;
    }

    IEnumerator ClearPlayerMessage()
    {
        yield return new WaitForSeconds(3);
        _playerMessageArea.text = "";
        _playerMessageZone.SetActive(false);
    }
}

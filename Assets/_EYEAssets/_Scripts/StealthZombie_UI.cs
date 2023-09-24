using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StealthZombie_UI : MonoBehaviour
{
    [SerializeField] private GameObject[] _menuPanels;

    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _creditsObject;
    [SerializeField] private GameObject _mainMenu;

    [SerializeField] private Image _brightnessPanel;
    [SerializeField] private Slider _brightnessSlider;



    void Start()
    {
        InitializeZombieUI();
    }

    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DisplayMainMenu();
        }
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

    public void DisplayMainMenu()
    {
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
        foreach (GameObject menu in _menuPanels)
        {
            menu.SetActive(false);
        }

        _menuPanels[2].SetActive(true);
        _buttonsPanel.SetActive(false);
    }

    public void DisplayCreditScreen()
    {
        foreach(GameObject go in _menuPanels)
        {
            go.SetActive(false);
        }
        _menuPanels[3].SetActive(true);
        _buttonsPanel.SetActive(true);
        _exitButton.SetActive(true);
        _creditsObject.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void AdjustBrightness()
    {
        var panelColor = _brightnessPanel.color;
        panelColor.a = _brightnessSlider.value;
        _brightnessPanel.color = panelColor;
    }



}

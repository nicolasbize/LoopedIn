using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public LogoScreen logoCanvas;
    public Canvas mainMenuCanvas;
    public IntroScreen introCanvas;
    public WakeUpCanvas wakeUpCanvas;

    public bool playLogos;
    public bool playIntro;
    

    public bool InMenu {  get; private set; }

    public event EventHandler OnStartNewGame;

    public MenuButton newGameButton;
    public MenuButton optionsButton;
    public MenuButton quitButton;

    public static MenuManager Instance;

    private void Awake() {
        Instance = this;
        InMenu = true;
    }

    public void StartGame() {
        // check if already done a bunch of things, and show end screen
        InMenu = false;
    }

    private void Start() {
        newGameButton.OnClick += NewGameButton_OnClick;
        optionsButton.OnClick += OptionsButton_OnClick;
        quitButton.OnClick += QuitButton_OnClick;
        logoCanvas.OnLogosComplete += OnLogosComplete;
        introCanvas.OnIntroComplete += OnIntroComplete;

        introCanvas.gameObject.SetActive(false);
        wakeUpCanvas.gameObject.SetActive(false);
        if (playLogos) {
            logoCanvas.gameObject.SetActive(true);
        } else {
            mainMenuCanvas.gameObject.SetActive(true);
        }
    }

    private void OnIntroComplete(object sender, EventArgs e) {
        introCanvas.gameObject.SetActive(false);
        wakeUpCanvas.gameObject.SetActive(true);
    }

    private void OnLogosComplete(object sender, EventArgs e) {
        logoCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }

    private void QuitButton_OnClick(object sender, EventArgs e) {
        Application.Quit();
    }

    private void OptionsButton_OnClick(object sender, EventArgs e) {
        // show options panel
    }

    private void NewGameButton_OnClick(object sender, EventArgs e) {
        mainMenuCanvas.gameObject.SetActive(false);
        if (playIntro) {
            introCanvas.gameObject.SetActive(true);
        } else {
            Player.Instance.ReceiveText();
        }

    }

}

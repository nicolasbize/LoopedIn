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
    public EndLoopCanvas endLoopCanvas;
    public Credits creditsCanvas;
    public Canvas cursorCanvas;

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

    public void TransitionToRestart() {
        endLoopCanvas.gameObject.SetActive(true);
        endLoopCanvas.StartEndLoop();
    }

    private void Start() {
        newGameButton.OnClick += NewGameButton_OnClick;
        optionsButton.OnClick += OptionsButton_OnClick;
        quitButton.OnClick += QuitButton_OnClick;
        logoCanvas.OnLogosComplete += OnLogosComplete;
        introCanvas.OnIntroComplete += OnIntroComplete;
        endLoopCanvas.OnReadyToPrepareLoop += EndLoopCanvas_OnReadyToPrepareLoop;
        endLoopCanvas.OnReadyForLoop += EndLoopCanvas_OnReadyForLoop;

        introCanvas.gameObject.SetActive(false);
        wakeUpCanvas.gameObject.SetActive(false);
        endLoopCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(false);
        cursorCanvas.gameObject.SetActive(false);
        if (playLogos) {
            logoCanvas.gameObject.SetActive(true);
        } else {
            mainMenuCanvas.gameObject.SetActive(true);
        }
    }

    public void FadeOutAndCredits() {
        creditsCanvas.gameObject.SetActive(true);
    }

    private void EndLoopCanvas_OnReadyToPrepareLoop(object sender, EventArgs e) {
        Debug.Log("Ready to prepare loop");
        Player.Instance.ResetToOriginalTransform();
        InMenu = true; // prevent moving
        endLoopCanvas.RestartGame();
    }

    private void EndLoopCanvas_OnReadyForLoop(object sender, EventArgs e) {
        Debug.Log("OnReadyForLoop");
        endLoopCanvas.gameObject.SetActive(false);
        wakeUpCanvas.gameObject.SetActive(true);
        wakeUpCanvas.GetComponent<Animator>().Play("WakeUp");

    }

    private void OnIntroComplete(object sender, EventArgs e) {
        introCanvas.gameObject.SetActive(false);
        wakeUpCanvas.gameObject.SetActive(true);
        cursorCanvas.gameObject.SetActive(true);
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
        GetComponent<AudioSource>().Stop();
        if (playIntro) {
            introCanvas.gameObject.SetActive(true);
        } else {
            Player.Instance.ReceiveText();
        }

    }

}

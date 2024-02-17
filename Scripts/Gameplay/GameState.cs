using DG.Tweening;
using System;
using UnityEngine;

public class GameState : MonoBehaviour, IInitializable
{
    public static GameState Instance;

    public Action GameReady;
    public Action GameStarted;
    public Action GamePaused;
    public Action GameUnpaused;
    public Action GameFinished;

    public Action ScoreAdded;

    public enum State
    { 
        InGame,
        Paused,
        Finished
    }
    public State CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Initialize()
    {
        StartGame();
    }
    public void StartGame()
    {
        GameStarted?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void PauseGame()
    {
        GamePaused?.Invoke();
        CurrentState = State.Paused;
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        GameUnpaused?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void FinishGame()
    {
        GameFinished?.Invoke();
        PlayerInput.Instance.IsEnabled = false;
        CurrentState = State.Finished;

        Camera.main.DOShakePosition(0.4f, 0.2f, fadeOut: true).SetUpdate(true);
        Camera.main.DOShakeRotation(0.4f, 0.2f, fadeOut: true).SetUpdate(true);
        //Time.timeScale = 0.0f;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);

        /*if (AudioVibrationManager.Instance.IsVibrationEnabled)
            Handheld.Vibrate();*/
    }
}
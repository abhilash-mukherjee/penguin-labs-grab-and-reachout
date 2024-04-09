using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public delegate void GameplayInitiationHandler(SessionData sessionData);
    public static event GameplayInitiationHandler OnGameplayInitiated, OnGameplayEnded, OnGameplayReset;
    private SessionData _currentSessionData;
    [SerializeField] private BoolData isActiveSessionPresent;
    [SerializeField] private GameConfig gameConfig;
    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += InitiateGamaplay;
        SessionManager.OnSessionEnded += EndGameplay;
    }
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= InitiateGamaplay;
        SessionManager.OnSessionEnded -= EndGameplay;
    }

    private void EndGameplay(SessionData data)
    {
        _currentSessionData = null;
        StartCoroutine(GamePlayEndCoroutine(gameConfig.PostSessionEndResetTime));
    }

    private void InitiateGamaplay(SessionData data)
    {
        OnGameplayReset?.Invoke(_currentSessionData);
        _currentSessionData = data;
        StartCoroutine(GamePlayStartCoroutine(gameConfig.SessionStartDelay));
    }

    IEnumerator GamePlayStartCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(isActiveSessionPresent.value == true) OnGameplayInitiated?.Invoke(_currentSessionData);
    }
    
    IEnumerator GamePlayEndCoroutine(float delay)
    {
        Debug.Log("Inside end coroutine Delay: " + delay);
        yield return new WaitForSeconds(delay);
        if (isActiveSessionPresent.value == false)
        {
            OnGameplayEnded?.Invoke(_currentSessionData);
        }
    }
}

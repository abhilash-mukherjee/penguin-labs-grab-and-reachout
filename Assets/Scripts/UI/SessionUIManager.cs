using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionUIManager : MonoBehaviour
{

    [SerializeField]
    private SessionUISuperClass sessionStartUI, gameplayUI, endUI;

    [SerializeField]
    private GameConfig gameConfig;

    private SessionData _currentSessionData;

    private void Start()
    {
        sessionStartUI.ShowUI(null);
    }
    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += NewSessionCreated;
        SessionManager.OnSessionEnded += SessionEnded;
    }
    
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= NewSessionCreated;
        SessionManager.OnSessionEnded -= SessionEnded;
    }


    private void NewSessionCreated(SessionData data)
    {
        _currentSessionData = data;
        sessionStartUI.ShowUI(data);
        endUI.HideUI(_currentSessionData);
        gameplayUI.HideUI(data);
        StopAllCoroutines();
        StartCoroutine(GameplayStarted());
    }

    IEnumerator GameplayStarted()
    {
        yield return new WaitForSeconds(gameConfig.SessionStartDelay);
        sessionStartUI.HideUI(_currentSessionData);
        endUI.HideUI(_currentSessionData);
        gameplayUI.ShowUI(_currentSessionData);
    }

    private void SessionEnded(SessionData data)
    {
        _currentSessionData = null;
        sessionStartUI.HideUI(null);
        gameplayUI.HideUI(_currentSessionData);
        endUI.ShowUI(_currentSessionData);
        StopAllCoroutines();
        StartCoroutine(ResetUI());
    }

    IEnumerator ResetUI()
    {
        yield return new WaitForSeconds(gameConfig.PostSessionEndResetTime);
        endUI.HideUI(_currentSessionData);
        sessionStartUI.ShowUI(null);
        gameplayUI.HideUI(_currentSessionData);
    }
}

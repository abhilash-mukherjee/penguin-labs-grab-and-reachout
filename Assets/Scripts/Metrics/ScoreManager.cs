using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private IntegerData score;
    [SerializeField] private GameConfig gameConfig;

    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += ResetScore;
    }
    
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= ResetScore;
    }

    private void ResetScore(SessionData data)
    {
        score.value = 0;
    }

}

using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private IntegerData score;
    [SerializeField] private GameConfig gameConfig;

    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += ResetScore;
        SphereController.OnSpherePlacedInBox += IncreaseScore;
    }
    
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= ResetScore;
        SphereController.OnSpherePlacedInBox -= IncreaseScore;
    }

    private void ResetScore(SessionData data)
    {
        score.value = 0;
    }

    private void IncreaseScore()
    {
        score.value += gameConfig.ScoreIncrement;
    }
}

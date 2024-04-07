using UnityEngine;

[CreateAssetMenu(fileName ="New Game Config", menuName ="Data Containers / Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _sessionStartDelay;
    [SerializeField] private int _calibrationTime;
    [SerializeField] private int _postSessionEndResetTime;
    [SerializeField] private float _getCurrentSessionFromServerInterval;
    [SerializeField] private int _scoreIncrementOnDodge;
    [SerializeField] private string _secret;
    [SerializeField] private string _moduleName = "LATERAL_MOVEMENT_MODULE";
    public URLHolder urlHolder;
    public int SessionStartDelay { get => _sessionStartDelay; }
    public int CalibrationTime{ get => _calibrationTime; }
    public int PostSessionEndResetTime { get => _postSessionEndResetTime; }
    public float GetCurrentSessionFromServerInterval { get => _getCurrentSessionFromServerInterval; }
    public int ScoreIncrementOnDodge { get => _scoreIncrementOnDodge;  }
    public string Secret { get => _secret; }
    public string ModuleName { get => _moduleName; }
}

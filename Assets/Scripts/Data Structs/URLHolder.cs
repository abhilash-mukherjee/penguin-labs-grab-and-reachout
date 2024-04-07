using UnityEngine;

[CreateAssetMenu(fileName ="New URL Holder", menuName = "Data Containers / URL Holder")]
public class URLHolder : ScriptableObject
{
    [SerializeField] private string _hTTPGetCurrentSessionRequestURL;
    [SerializeField] private string _hTTPEndSessionRequestURL;
    [SerializeField] private string _hTTPStartSessionRequestURL;
    [SerializeField] private string _hTTPSendMetricsRequestURL;
    public string HTTPGetCurrentSessionRequestURL { get => _hTTPGetCurrentSessionRequestURL; }
    public string HTTPEndSessionRequestURL { get => _hTTPEndSessionRequestURL; }
    public string HTTPStartSessionRequestURL { get => _hTTPStartSessionRequestURL; }
    public string HTTPSendMetricsRequestURL { get => _hTTPSendMetricsRequestURL; }
}
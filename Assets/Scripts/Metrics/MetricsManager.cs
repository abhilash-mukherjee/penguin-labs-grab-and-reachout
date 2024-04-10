using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class MetricsManager : MonoBehaviour
{
    [SerializeField] private IntegerData score;
    [SerializeField] private SessionMetrics sessionMetrics;
    [SerializeField] private GameConfig gameConfig;
    private int _leftCubes, _rightCubes, _leftDodges, _rightDodges, _leftHits, _rightHits;

    private void OnEnable()
    {
        SessionManager.OnSessionEnded += SendMetrics;
        SessionManager.OnNewSessionCreated += ResetMetrics;
    }
    private void OnDisable()
    {
        SessionManager.OnSessionEnded -= SendMetrics;
        SessionManager.OnNewSessionCreated -= ResetMetrics;
    }

    private void ResetMetrics(SessionData data)
    {
        score.value = 0;
    }

    private void SendMetrics(SessionData data)
    {
        StartCoroutine(MetricsCoroutine(data.id));
    }

    IEnumerator MetricsCoroutine(string sessionId)
    {
        string uri = gameConfig.urlHolder.HTTPSendMetricsRequestURL;
        Debug.Log("#################Inside metrics coroutine. id =" + sessionId);
        string jsonData = "{\"id\":" + $" \"{sessionId}\"" + "," +  " \"sessionMetrics\": {"
                   + "\"score\": " + score.value
                   + "}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("unity-client-secret", gameConfig.Secret);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
        }
    }


}

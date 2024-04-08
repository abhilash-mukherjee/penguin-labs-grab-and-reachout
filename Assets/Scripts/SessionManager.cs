using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Text;

public class SessionManager : MonoBehaviour
{
    public delegate void SessionDataHandler(SessionData data);
    public static event SessionDataHandler OnNewSessionCreated, OnSessionPaused, OnSessionResumed, OnSessionEnded;

    [SerializeField]
    private GameConfig gameConfig;
    [SerializeField]
    public BoolData isActiveSessionPresent;

    private SessionData _sessionData;

    void Start()
    {
        _sessionData = new SessionData();
        ResetSessionData();
        StartCoroutine(RecursiveCoroutine());
    }


    private void SessionCompleted(SessionData data)
    {
        if (data.id != _sessionData.id) return;
        EndSession();
        PostRequest(gameConfig.urlHolder.HTTPEndSessionRequestURL);
    }

    IEnumerator RecursiveCoroutine()
    {
        yield return new WaitForSecondsRealtime(gameConfig.GetCurrentSessionFromServerInterval);
        GetRequest(gameConfig.urlHolder.HTTPGetCurrentSessionRequestURL);
        yield return StartCoroutine(RecursiveCoroutine());
    }

    void PostRequest(string uri)
    {
        var webRequest = new UnityWebRequest(uri, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // Add the required header
        webRequest.SetRequestHeader("unity-client-secret", gameConfig.Secret);

        webRequest.SendWebRequest();
    }

    void GetRequest(string uri)
    {
        var webRequest = UnityWebRequest.Get(uri);
        webRequest.SetRequestHeader("unity-client-secret", gameConfig.Secret);
        webRequest.SendWebRequest().completed += (op) => { ProcessData(webRequest); };

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period)) SessionCompleted(_sessionData);
    }
    void ProcessData(UnityWebRequest webRequest)
    {
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:

            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + webRequest.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + webRequest.error);
                break;

            case UnityWebRequest.Result.Success:
                {
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(webRequest.downloadHandler.text);

                    //Ongoing Session Ended
                    if (responseData.sessionData == null || string.IsNullOrEmpty(responseData.sessionData.id))
                    {
                        if (!string.IsNullOrEmpty(_sessionData.id))
                        {
                            EndSession();
                        }
                        else
                        {
                            Debug.Log("No session anywhere");
                            return;
                        }
                    }

                    //New session detected
                    else if (!string.IsNullOrEmpty(responseData.sessionData.id) && string.IsNullOrEmpty(_sessionData.id)
                        && responseData.sessionData.status == "NOT_STARTED" && responseData.sessionData.module == gameConfig.ModuleName)
                    {
                        _sessionData = responseData.sessionData;
                        _sessionData.status = "RUNNING";
                        isActiveSessionPresent.value = true;
                        PostRequest(gameConfig.urlHolder.HTTPStartSessionRequestURL);
                        OnNewSessionCreated?.Invoke(_sessionData);
                        Debug.Log($"New Session: {_sessionData.id}; CREATED");
                    }

                    else if(responseData.sessionData.module != gameConfig.ModuleName)
                    {
                        Debug.LogError("Current session is of a different module");
                    }
                    else if (responseData.sessionData.id != _sessionData.id)
                    {
                        Debug.LogError("A different session request sent while this session is running.");
                    }

                    Debug.Log("Mega Log:\n" +
                                "Received: " + webRequest.downloadHandler.text + "\n" +
                                "Module: " + responseData.sessionData.module + "\n" +
                                "Code: " + responseData.sessionData.id + "\n"
                             );
                }
                break;
        }
    }

    private void EndSession()
    {
        isActiveSessionPresent.value = false;
        OnSessionEnded?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.id}; ENDED");
        ResetSessionData();
    }

    private void ResetSessionData()
    {
        _sessionData.id = "";
        _sessionData.sessionParams = null;
        _sessionData.status = "";
        _sessionData.module = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Box
{
    public float boxX;
    public float boxZ;
    public string label;
    public string colorLight;
    public string colorDark;
}

[System.Serializable]
public class Sphere
{
    public float spawnCentreX;
    public float spawnCentreZ;
    public float zoneWidth;
    public string color;
    public string label;
}

[System.Serializable]
public class SessionParams
{
    public string targetHand;
    public int reps;
    public Box[] boxes;
    public Sphere[] spheres;
}

[System.Serializable]
public class SessionData
{
    public string module;
    public string id;
    public SessionParams sessionParams;
    public string status;
}

[System.Serializable]
public class ResponseData
{
    public SessionData sessionData;
}

public enum SessionStatus
{
    NO_ACTIVE_SESSION,
    SESSION_RUNNING,
    SESSION_PAUSED
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoxSpawnner : MonoBehaviour
{
    public delegate void BoxRemovalHandler();
    public static event BoxRemovalHandler OnBoxesRemoved;
    [SerializeField] private BoxController boxPrefab;
    private void OnEnable()
    {
        GameplayManager.OnGameplayInitiated += SpawnBoxes;
        GameplayManager.OnGameplayEnded += RemoveBoxes;
        GameplayManager.OnGameplayReset += RemoveBoxes;
    }
    
    private void OnDisable()
    {
        GameplayManager.OnGameplayInitiated -= SpawnBoxes;
        GameplayManager.OnGameplayEnded -= RemoveBoxes;
        GameplayManager.OnGameplayReset -= RemoveBoxes;
    }

    private void RemoveBoxes(SessionData sessionData)
    {
        OnBoxesRemoved?.Invoke();
    }

    private void SpawnBoxes(SessionData sessionData)
    {
        for(int i = 0; i < sessionData.sessionParams.boxes.Length; i++)
        {
            var boxPrefab = Instantiate(this.boxPrefab);
            boxPrefab.GetComponent<BoxController>().SpawnBox(sessionData.sessionParams.boxes[i]);
        }
    }
}

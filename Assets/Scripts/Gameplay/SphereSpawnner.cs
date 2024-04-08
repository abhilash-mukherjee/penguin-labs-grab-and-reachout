using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereSpawnner : MonoBehaviour
{
    [SerializeField] private float sphereY;
    [SerializeField] private SphereController spherePrefab;
    private List<Queue<Vector3>> spawnPointQueues = new();
    private Sphere[] _spheres;
    private int _currentSphereIndex = 0;
    private void OnEnable()
    {
        GameplayManager.OnGameplayInitiated += ConfigureSpawnningData;
    }
    
    private void OnDisable()
    {
        GameplayManager.OnGameplayInitiated -= ConfigureSpawnningData;
    }

    public void SpawnNextSphere()
    {
        bool spawned = false;
        int attempts = 0;

        while (!spawned && attempts < _spheres.Length)
        {
            Queue<Vector3> currentQueue = spawnPointQueues[_currentSphereIndex];

            if (currentQueue.Count > 0)
            {
                // Dequeue the position for the current sphere
                Vector3 spawnPosition = currentQueue.Dequeue();

                // Instantiate the sphere prefab and initialize it
                SphereController newSphere = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
                newSphere.InitiateSphere(_spheres[_currentSphereIndex], spawnPosition);

                spawned = true; // Mark as spawned
            }
            else
            {
                // Move to the next sphere variant if the current queue is empty
                _currentSphereIndex = (_currentSphereIndex + 1) % _spheres.Length;
            }

            attempts++;
        }

        if (!spawned)
        {
            Debug.Log("Session ended - no more spheres to spawn.");
        }
        else
        {
            // Prepare the index for the next call to this function
            _currentSphereIndex = (_currentSphereIndex + 1) % _spheres.Length;
        }
    }

    private void ConfigureSpawnningData(SessionData sessionData)
    {
        _spheres = sessionData.sessionParams.spheres;
        var reps = sessionData.sessionParams.reps;
        for(int i = 0; i < _spheres.Length; i++)
        {
            var sphere = _spheres[i];
            var zoneCentre = new Vector3(sphere.spawnCentreX, sphereY, sphere.spawnCentreZ);
            int sphereCount = GetSphereCount(reps, _spheres.Length, i);
            spawnPointQueues.Add(GetSpawnPointsQueue(sessionData.sessionParams.reps, zoneCentre, sphere.zoneWidth));
        }
    }

    private int GetSphereCount(int reps, int sphereVariants, int i)
    {
        if(i == sphereVariants - 1)
        {
            return reps / sphereVariants + reps % sphereVariants;
        }
        return reps / sphereVariants;
    }

    private Queue<Vector3> GetSpawnPointsQueue(int sphereCount, Vector3 zoneCentre, float zoneWidth)
    {
        // Calculate the half width to easily calculate the grid centers
        float halfWidth = zoneWidth / 2f;
        float thirdWidth = zoneWidth / 3f; // One third of the zone width, for grid division

        // Calculate the starting point (bottom left of the square)
        Vector3 startPoint = zoneCentre - new Vector3(halfWidth, 0, halfWidth);

        // List to hold the center points of the 9 grids
        List<Vector3> gridCenters = new List<Vector3>();

        // Loop to calculate the center points of the 9 grids
        for (int i = 0; i < 3; i++) // Rows
        {
            for (int j = 0; j < 3; j++) // Columns
            {
                // Calculate the center point of the current grid
                Vector3 gridCenter = startPoint + new Vector3(thirdWidth / 2 + j * thirdWidth, 0, thirdWidth / 2 + i * thirdWidth);
                gridCenters.Add(gridCenter);
            }
        }

        var spawnPoints = new Queue<Vector3>();
        // Enqueue points into the spawnPoints queue
        for (int i = 0; i < sphereCount; i++)
        {
            // Pick a random grid center point
            int randomIndex = Random.Range(0, gridCenters.Count);
            Vector3 spawnPoint = gridCenters[randomIndex];

            // Enqueue the selected point
            spawnPoints.Enqueue(spawnPoint);
        }
        return spawnPoints;
    }


    //for test only
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnNextSphere();
        }
    }
}

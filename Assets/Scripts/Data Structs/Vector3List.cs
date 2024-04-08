using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Vector3 List", menuName ="Shared Data/ Vector3 List")]
public class Vector3List : ScriptableObject
{
    [SerializeField] private List<Vector3> pointCloud = new();
    public void AddToList(Vector3 point)
    {
        pointCloud.Add(point);
    }
    public void ClearList()
    {
        pointCloud.Clear();
    }


}

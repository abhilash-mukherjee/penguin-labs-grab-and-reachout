using System;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public string _label;
    [SerializeField] private float boxY;
    [SerializeField] private MeshRenderer modelMeshRenderer;
    [SerializeField] private GameObject boxModel;
    [SerializeField] private float boxLocalScale = 0.75f;
    public string Label { get => _label; }

    private void Awake()
    {
        boxModel.SetActive(false);
    }
    private void OnEnable()
    {
        BoxSpawnner.OnBoxesRemoved += Remove;
    }
    private void OnDisable()
    {
        BoxSpawnner.OnBoxesRemoved -= Remove;
    }

    private void Remove()
    {
        Destroy(gameObject);
    }

    public void SpawnBox(Box box)
    {
        transform.position = new Vector3(box.boxX, boxY, box.boxZ);
        transform.localScale = boxLocalScale * Vector3.one;
        _label = box.label;
        var colors = new Color[2];
        colors[0]  = HelperMethods.ParseHexStringToColor(box.colorLight);
        colors[1] = HelperMethods.ParseHexStringToColor(box.colorDark);
        modelMeshRenderer.materials = HelperMethods.GetMaterialArrayFromColors( colors);
        boxModel.SetActive(true);
    }
}
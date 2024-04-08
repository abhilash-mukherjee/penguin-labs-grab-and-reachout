using UnityEngine;

public class SphereController : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private MeshRenderer meshRenderer;
    private string _label;

    public string Label { get => _label; }

    public void InitiateSphere(Sphere sphere, Vector3 finalPosition)
    {
        _label = sphere.label;
        var colors = new Color[1];
        colors[0] = HelperMethods.ParseHexStringToColor(sphere.color);
        meshRenderer.materials = HelperMethods.GetMaterialArrayFromColors(colors);
        InitiateAnimation(finalPosition);
    }

    private void InitiateAnimation(Vector3 finalPosition)
    {
        model.SetActive(true);
        transform.position = finalPosition;
    }
}
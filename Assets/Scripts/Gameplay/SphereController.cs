using UnityEngine;

public class SphereController : MonoBehaviour
{
    public delegate void SphereEventHandler();
    public static event SphereEventHandler OnSpherePickup, OnSpherePlacedInBox;
    [SerializeField] private string handTag, boxTag, floorTag;
    [SerializeField] private GameObject model;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int physicsLayer = 8;
    [SerializeField] private float scaleReductionOnPickup = 0.75f, scaleReductionOnDrop = 0.5f;
    public string _label;
    public string _targetHand;
    private bool _isLiftedOnce;
    public string Label { get => _label; }

    private void OnEnable()
    {
        GameplayManager.OnGameplayEnded += DestroySpheres;
        GameplayManager.OnGameplayReset += DestroySpheres;
    }
    
    private void OnDisable()
    {
        GameplayManager.OnGameplayEnded -= DestroySpheres;
        GameplayManager.OnGameplayReset -= DestroySpheres;
    }

    private void DestroySpheres(SessionData sessionData)
    {
        Destroy(gameObject);
    }

    public void InitiateSphere(Sphere sphere, Vector3 finalPosition, string targetHand)
    {
        _label = sphere.label;
        _targetHand = targetHand;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(handTag) && !_isLiftedOnce)
        {
            
            if(other.TryGetComponent<ColliderLabel>(out var colliderLabel))
            {
                if(colliderLabel.label == _targetHand)
                {
                    transform.parent = other.transform.GetChild(0);
                    transform.localPosition = Vector3.zero;
                    transform.localScale = transform.localScale * scaleReductionOnPickup;
                    _isLiftedOnce = true;
                    OnSpherePickup?.Invoke();
                }
                else
                {
                    Debug.Log("Wrong hand");
                }
            }
        }
        if (other.gameObject.CompareTag(boxTag))
        {
            Debug.Log("**Collided with box entry collider");
            if (other.TryGetComponent<BoxController>(out var boxController))
            {
                if(boxController.Label == _label)
                {
                    transform.parent = null;
                    gameObject.layer = physicsLayer;
                    transform.localScale = transform.localScale * scaleReductionOnDrop;
                    Debug.Log("set layer to " + gameObject.layer);
                    var rb = GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    OnSpherePlacedInBox?.Invoke();
                }
                else
                {
                    Debug.Log("wrong box");
                }
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(floorTag))
        {
            Debug.Log("#COllided with floor");
            if(transform.TryGetComponent<Rigidbody>(out var rb) && transform.TryGetComponent<Collider>(out var collider))
            {
                Destroy(rb);
                Destroy(collider);
            }
        }
    }
}

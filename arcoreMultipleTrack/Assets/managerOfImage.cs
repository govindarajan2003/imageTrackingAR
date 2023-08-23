using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class MultiImageTrackingController : MonoBehaviour
{
    [System.Serializable]
    public struct ImagePrefabPair
    {
        public XRReferenceImage referenceImage;
        public GameObject prefab;
    }

    [SerializeField] private List<ImagePrefabPair> imagePrefabPairs = new List<ImagePrefabPair>();
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    private Dictionary<XRReferenceImage, GameObject> imagePrefabMap = new Dictionary<XRReferenceImage, GameObject>();

    private void Start()
    {
        foreach (var pair in imagePrefabPairs)
        {
            imagePrefabMap[pair.referenceImage] = pair.prefab;
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (imagePrefabMap.TryGetValue(trackedImage.referenceImage, out GameObject prefab))
            {
                PlacePrefab(trackedImage, prefab);
            }
        }
    }

    private void PlacePrefab(ARTrackedImage trackedImage, GameObject prefab)
    {
        GameObject newPrefab = Instantiate(prefab, trackedImage.transform.position, Quaternion.identity);
        // You might need to adjust rotation, scale, and other properties here
    }
}

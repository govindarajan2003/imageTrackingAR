using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracking : MonoBehaviour
{
    [SerializeField] private GameObject[] placeablePrefab;
    
    public Dictionary<string, GameObject> spawnedPrefab = new Dictionary<string, GameObject>();

    public ARTrackedImageManager trackedImageManager;




    public void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        

        foreach(GameObject prefab in placeablePrefab)
        {
            GameObject newPrefab= Instantiate(prefab,Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefab.Add(prefab.name, newPrefab);
        }
    }
    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        
        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefab[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedPrefab[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach(GameObject go in spawnedPrefab.Values)
        {
            if(go.name != name)
            {
                go.SetActive(false);
            }
        }
    }
}
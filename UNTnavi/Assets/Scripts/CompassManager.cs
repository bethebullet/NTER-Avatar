using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CompassManager : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public ARPlaneManager arPlaneManager;
    public GameObject rocketPrefab;

    private bool rocketCreated = false;
    private GameObject instantiatedRocket;

    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
    
    void Update()
    {
          if (!rocketCreated)
                        {
                            //Rraycast Planes
                            if (arRaycastManager.Raycast(Camera.main.transform.position, arRaycastHits))
                            {
                                var pose = arRaycastHits[0].pose;
                                //CreateRocket(pose.position);
                                Vector3 spawnPos = Camera.main.transform.position;
                                spawnPos.y = spawnPos.y - 1f;
                                CreateRocket(spawnPos);
                                TogglePlaneDetection(false);
                                return;
                            }
                        }
    }
    
    private void CreateRocket(Vector3 position)
    {
        instantiatedRocket = Instantiate(rocketPrefab, position, Quaternion.identity);
        rocketCreated = true;
    }

    private void TogglePlaneDetection(bool state)
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(state);
        }
        arPlaneManager.enabled = state;
    }


    public void DeleteRocket(GameObject cubeObject)
    {
        Destroy(cubeObject);
        rocketCreated = false;
        TogglePlaneDetection(true);
    }
    
}
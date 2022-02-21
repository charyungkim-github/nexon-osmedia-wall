using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    TrackingManager trackingManager;
    NetworkManager networkManager;

    void Start()
    {
      trackingManager = GetComponentInChildren<TrackingManager>();
      networkManager = GetComponentInChildren<NetworkManager>();
    }

    void Update()
    {
        
    }

    public void SendTrackingData(List<bool> resultData) {
      networkManager.SendTrackingData(resultData);
    }
}

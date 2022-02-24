using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;

public class test : MonoBehaviour
{
  public RsDevice device;
  public RsDevice device1;
  
  void Start()
  {

    Intel.RealSense.Device d = device.ActiveProfile.Device;
    foreach (var s in d.Sensors) {
      
      // Debug.Log(s.Info[CameraInfo.SerialNumber]);
    }
  }

  
  void Update()
  {
    // Debug.Log(device.ActiveProfile.Device);
    
  }
}

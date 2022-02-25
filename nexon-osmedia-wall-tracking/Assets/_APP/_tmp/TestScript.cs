using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TestScript : MonoBehaviour
{
  public RsDevice device;
  public RawImage sourceFeed;
  
  void Start() {
    string serialNumber = "12345";
    int width = 1280;
    int height = 720;
    
    device.DeviceConfiguration.RequestedSerialNumber = serialNumber;
    device.DeviceConfiguration.Profiles[0].Width = width;
    device.DeviceConfiguration.Profiles[0].Height = height;
    
    RsStreamTextureRenderer deviceRenderer = device.GetComponentInChildren<RsStreamTextureRenderer>();
    // NEED TO SET TEXTURE BINDING
    // Action<Texture> callback;
    // UnityEngine.Events.UnityAction<Texture> test;
    // deviceRenderer.textureBinding.AddListener(test(sourceFeed.texture));
  }

  void Update() {
      
  }
}

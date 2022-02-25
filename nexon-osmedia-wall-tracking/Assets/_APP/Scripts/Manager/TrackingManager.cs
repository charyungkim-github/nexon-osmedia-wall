﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour
{
  public Camera trackingCamera;
  public RawImage cameraFeed;
  public Transform deviceHolder;
  public GameObject devicePrefab;
  public Transform sourceFeedHolder;
  public GameObject sourceFeedPrefab;
  
  int width, height, rows, cols;
  float depthThreshold;
  public bool debugGUI = true;

  Manager manager;
  RenderTexture cameraRenderTexture;
  Texture2D trackingTexture;
  List<bool> resultData = new List<bool>();
  int pixelWidth, pixelHeight;

  // debug
  List<float> colorValues = new List<float>();
  GUIStyle style = new GUIStyle();
  bool isOnSetting = false;

  
  #region Basic Functions
  void Start() { 

    // manager
    manager = GetComponentInParent<Manager>();

    // reset
    Reset();

    // gui style
    style.alignment = TextAnchor.MiddleCenter;
    style.normal.textColor = Color.red;

  }

  void Update() {

    if(isOnSetting) return;
    
    // tracking
    UpdateTexture();
    GetTextureColors();

    // send result
    manager.SendTrackingData(resultData);
  }
  #endregion


  #region Activatet Tracking
  public void ToggleActive(bool _isOnSetting) {
    isOnSetting = _isOnSetting;
    if(isOnSetting) cameraFeed.gameObject.SetActive(false);
    else cameraFeed.gameObject.SetActive(true);
  }

  public void Reset() {
        
    for(int i=0; i<Data.Camera.Count; i++) {
      // create canvas
      if(i >= sourceFeedHolder.childCount) {
        GameObject instancedFeed = Instantiate(sourceFeedPrefab);
        instancedFeed.transform.SetParent(sourceFeedHolder);
      }
      
      // set properties
      RectTransform sourceFeed = sourceFeedHolder.GetChild(i).GetComponent<RectTransform>();      
      sourceFeed.sizeDelta = new Vector2(1920, 1080); // need calib data ? need calculate by resolution / count?
      
      // create device
      if(i >= deviceHolder.childCount) {
        GameObject instancedDevice = Instantiate(devicePrefab);
        instancedDevice.transform.SetParent(deviceHolder);
      }
      
      // set properties
      RsDevice device = deviceHolder.GetChild(i).GetComponent<RsDevice>();
      device.enabled = false;
      device.DeviceConfiguration.RequestedSerialNumber = Data.Camera[i].serialNumber;
      device.DeviceConfiguration.Profiles[0].Width = Data.Camera[i].width;
      device.DeviceConfiguration.Profiles[0].Height = Data.Camera[i].height;
      device.enabled = true;
      
      /* TODO */
      // RsStreamTextureRenderer deviceRenderer = device.GetComponentInChildren<RsStreamTextureRenderer>();
      // deviceRenderer.textureBinding ==========> SET sourceFeed.texture
    }    
    
    // set size
    width = Data.Tracking.width;
    height = Data.Tracking.height;
    rows = Data.Tracking.rows;
    cols = Data.Tracking.cols;
    depthThreshold = Data.Tracking.depthThreshold;

    // pixel size    
    pixelWidth = width / cols;
    pixelHeight = height / rows;

    // setup textures    
    if(cameraRenderTexture != null) cameraRenderTexture.Release();    
    if(trackingTexture == null) trackingTexture = new Texture2D(0, 0);
    cameraRenderTexture = new RenderTexture(width, height, 24);
    trackingTexture.Resize(width, height);
    trackingCamera.targetTexture = cameraRenderTexture;
    cameraFeed.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    cameraFeed.texture = cameraRenderTexture;
  }
  #endregion


  #region Convert Image
  void UpdateTexture() {

    // copy render texture to texture2d
    RenderTexture.active = null;
    RenderTexture.active = cameraRenderTexture;
    trackingTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    trackingTexture.Apply();
  }

  void GetTextureColors() {
    
    // reset
    resultData.Clear();    
    colorValues.Clear();

    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {

        if(x + pixelWidth > width) continue;
        
        // get color
        Color color = Utils.GetCenterColor((Texture2D)trackingTexture, x, y, pixelWidth, pixelHeight);

        // save result          
        resultData.Add(color.r > depthThreshold);
        colorValues.Add(color.r);
      }
    }
  }
  #endregion


  #region Debug
  void OnGUI() {
    if(isOnSetting) return;
    if(!Data.Tracking.debug) return;
    if(resultData.Count < 1) return;

    int index = 0;
    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {

        if(x + pixelWidth > width) continue;
        
        // string val = string.Format("{0:N1}", colorValues[index]);
        style.normal.textColor = resultData[index] ? Color.red : Color.gray;
        GUI.Box(new Rect(x, y, pixelWidth, pixelHeight), index.ToString(), style);
        index ++;
      }
    }
  }
  #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour
{
  public Camera trackingCamera;
  public RawImage cameraFeed;
  public Transform depthCameraHolder;
  public GameObject depthCameraPrefab;  
  public RsColorizer rsColorizer;

  Manager manager;
  RenderTexture cameraRenderTexture;
  Texture2D trackingTexture;
  List<bool> resultData = new List<bool>();
  int width, height;
  int pixelWidth, pixelHeight;
  int rows, cols;
  float depthThreshold;

  // profile
  bool isProfileInitialized = false;

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

    // on first setup :: need to wait until rs device connected
    if(depthCameraHolder.childCount > 0 && !isProfileInitialized) {
      if(depthCameraHolder.GetChild(0).GetComponent<RsDevice>().Streaming) {
        SetupProfile();
        isProfileInitialized = true;
      }
    }
  }
  #endregion


  #region Activatet Tracking
  public void ToggleActive(bool _isOnSetting) {
    isOnSetting = _isOnSetting;
    if(isOnSetting) cameraFeed.gameObject.SetActive(false);
    else cameraFeed.gameObject.SetActive(true);
  }

  public void Reset() {
      
    // create depth camera
    for(int i=0; i<Data.Camera.cameraData.Count; i++) {

      // create depth camera
      if(i >= depthCameraHolder.childCount) {
        GameObject depthCamera = Instantiate(depthCameraPrefab);
        depthCamera.transform.SetParent(depthCameraHolder);
      }

      // target depth camera      
      Transform targetDepthCamera = depthCameraHolder.GetChild(i);

      if(!manager.debugDevice) {
        
        // set device properties
        RsDevice device = targetDepthCamera.GetComponentInChildren<RsDevice>();
        device.DeviceConfiguration.RequestedSerialNumber = Data.Camera.cameraData[i].serialNumber;
        device.transform.position = Data.Camera.cameraData[i].position;
        device.transform.eulerAngles = Data.Camera.cameraData[i].rotation;
        device.transform.localScale = Data.Camera.cameraData[i].scale;
        device.DeviceConfiguration.Profiles[0].Width = Data.Camera.resolutionIndex == 0 ? 1280 : 640;      
        device.DeviceConfiguration.Profiles[0].Height = Data.Camera.resolutionIndex == 0 ? 720 : 480;

        // set active after setup device
        device.gameObject.SetActive(true);

        // setup profile
        SetupProfile();
      }
    }

    // remove unused depth camera
    for(int j=Data.Camera.cameraData.Count; j<depthCameraHolder.childCount; j++) {
      Destroy(depthCameraHolder.GetChild(j).gameObject);
    }

    // setup tracking camera
    trackingCamera.transform.position = Data.Camera.position;
    trackingCamera.orthographicSize = Data.Camera.size;
    trackingCamera.nearClipPlane = Data.Camera.near;
    trackingCamera.farClipPlane = Data.Camera.far;

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

  void SetupProfile() {
    rsColorizer.colorScheme = RsColorizer.ColorScheme.WhiteToBlack;
    rsColorizer.minDist = Data.Tracking.profileNear;
    rsColorizer.maxDist = Data.Tracking.profileFar;
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
        if(y + pixelHeight > height) continue;
        
        if(!manager.debugTracking){
          
          // get color
          Color color = Utils.GetAvrColor((Texture2D)trackingTexture, x, height - y, pixelWidth/2, pixelHeight/2);

          // save result          
          resultData.Add(color.r >= depthThreshold);
          colorValues.Add(color.r);
        }        
        else {
          
          // get mouse position
          Vector2 centerPosition = new Vector2(x + (pixelWidth/2), y + (pixelHeight/2));
          Vector2 mousePosition = new Vector2(Input.mousePosition.x, height - Input.mousePosition.y);
          float distance = Vector2.Distance(centerPosition, mousePosition);
          
          // sava data
          resultData.Add(distance < 200);
          colorValues.Add(distance);
        }
      }
    }
  }
  #endregion

  #region Debug
  void OnGUI() {
    if(isOnSetting) return;
    if(!manager.debugGUI) return;
    if(resultData.Count < 1) return;

    int index = 0;
    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {

        if(x + pixelWidth > width) continue;
        if(y + pixelHeight > height) continue;
        
        // string val = string.Format("{0}:{1:N2}", index,colorValues[index]);
        string val = string.Format("{0:N2}", colorValues[index]);
        style.normal.textColor = resultData[index] ? Color.red : Color.gray;
        GUI.Box(new Rect(x, y, pixelWidth, pixelHeight), val.ToString(), style);
        index ++;
      }
    }
  }
  #endregion
}

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

    // create depth camera
    for(int i=0; i<Data.Camera.cameraData.Count; i++) {

      // create depth camera
      if(i >= depthCameraHolder.childCount) {
        GameObject depthCamera = Instantiate(depthCameraPrefab);
        depthCamera.transform.SetParent(depthCameraHolder);
      }

      // target depth camera      
      Transform targetDepthCamera = depthCameraHolder.GetChild(i);

      // set canvas properties
      RectTransform sourceFeed = targetDepthCamera.GetComponent<RectTransform>();      
      sourceFeed.localPosition = Vector3.zero;
      sourceFeed.localScale = Vector3.one;
      // sourceFeed.anchoredPosition = new Vector2(Data.Camera.cameraData[i].positionX, Data.Camera.cameraData[i].positionY);
      sourceFeed.anchoredPosition = Utils.GetAnchoredPosition(Data.Camera.cameraData[i]);
      sourceFeed.sizeDelta = new Vector2(Data.Camera.cameraData[i].width, Data.Camera.cameraData[i].height); 
      sourceFeed.eulerAngles = new Vector3(0,0,Data.Camera.cameraData[i].rotationZ);
      
      if(!manager.isOnDebugDevice) {
        
        // set device properties
        targetDepthCamera.GetChild(0).gameObject.SetActive(true);
        RsDevice device = targetDepthCamera.GetComponentInChildren<RsDevice>();
        device.DeviceConfiguration.RequestedSerialNumber = Data.Camera.cameraData[i].serialNumber;
        device.DeviceConfiguration.Profiles[0].Width = Data.Camera.resolutionIndex == 0 ? 1280 : 640;      
        device.DeviceConfiguration.Profiles[0].Height = Data.Camera.resolutionIndex == 0 ? 720 : 480;
      }
    }

    // remove unused depth camera
    for(int j=Data.Camera.cameraData.Count; j<depthCameraHolder.childCount; j++) {
      Destroy(depthCameraHolder.GetChild(j).gameObject);
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
        
        if(!manager.isOnDebugTracking){
          
          // get color
          Color color = Utils.GetCenterColor((Texture2D)trackingTexture, x, y, pixelWidth, pixelHeight);

          // save result          
          resultData.Add(color.r > depthThreshold);
          colorValues.Add(color.r);
        }        
        else {
          
          // get mouse position
          Vector2 centerPosition = new Vector2(x + (pixelWidth/2), y + (pixelHeight/2));
          Vector2 mousePosition = new Vector2(Input.mousePosition.x, Data.Tracking.height-Input.mousePosition.y);
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
    if(!Data.Tracking.debug) return;
    if(resultData.Count < 1) return;

    int index = 0;
    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {

        if(x + pixelWidth > width) continue;
        
        string val = string.Format("{0:N1}", colorValues[index]);
        style.normal.textColor = resultData[index] ? Color.red : Color.gray;
        GUI.Box(new Rect(x, y, pixelWidth, pixelHeight), index.ToString(), style);
        index ++;
      }
    }
  }
  #endregion
}

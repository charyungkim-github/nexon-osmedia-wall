using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  #region Public Variables
  [Header("Manager")]
  public TrackingManager trackingManager;

  [Header("General")]
  public GameObject settingsHolder;
  public GameObject[] settings;
  public Image[] tabs;

  [Header("Network")]
  public Text networkStatus;
  public Text networkConnection;
  public InputfieldController ip;
  public InputfieldController port;

  [Header("Tracking")]
  public Text trackingStatus;
  public InputfieldController width;
  public InputfieldController height;
  public InputfieldController rows;
  public InputfieldController cols;
  public InputfieldController depthThreshold;
  public InputfieldController profileNear;
  public InputfieldController profileFar;

  [Header("Camera")]
  public CameraSettingsController cameraSettingsController;
  public Dropdown cameraResolution;
  public InputfieldController cameraPositionX;
  public InputfieldController cameraPositionY;
  public InputfieldController cameraPositionZ;
  public InputfieldController size;
  public InputfieldController near;
  public InputfieldController far;
  #endregion


  #region Private Variables
  Manager manager;
  bool isOnSetting = false;
  int activatedIndex = -1;
  #endregion

  #region Basic Functions
  void Start() {

    // manager
    manager = GetComponentInParent<Manager>();

    // event listner
    AddDebugEventListner() ;

    // reset
    CloseSetting();
  }

  void Update() {
    // check for network status
    if(isOnSetting) networkConnection.text = Data.Network.isConnected.ToString();

    // check for device status
    if(isOnSetting) cameraSettingsController.UpdateDepthCameraStatus();
  }

  
  #endregion


  #region  Activate Settings
  public void ToggleActive(bool _isOnSetting) {
    isOnSetting = _isOnSetting;
    if(isOnSetting) OpenSettings(0);
    else CloseSetting();
  }

  public void OpenSettings(int index) {
    
    if(index == activatedIndex) return;

    // open setting
    if(activatedIndex == -1) {
      settingsHolder.SetActive(true);
      LoadData();
    }

    // change tab
    else if(activatedIndex > -1) {
      settings[activatedIndex].SetActive(false);
      tabs[activatedIndex].color = Color.gray;
    }

    // set active target
    activatedIndex = index;
    settings[activatedIndex].SetActive(true);
    tabs[activatedIndex].color = Color.white;
  }

  void CloseSetting() {
    
    activatedIndex = -1;
    settingsHolder.SetActive(false);    
    for(int i=0; i<settings.Length; i++) {
      settings[i].SetActive(false);
      tabs[i].color = Color.gray;
    }
  }
  #endregion

  #region Save and Load Data
  public void LoadData() {

    // network
    networkStatus.text = string.Format("{0}, {1}", Data.Network.ip, Data.Network.port);
    ip._string = Data.Network.ip;
    port._string = Data.Network.port;

    // tracking
    trackingStatus.text = string.Format(
      "size : {0} x {1}, cells : {2} x {3}, thresold : {4}", 
      Data.Tracking.width, Data.Tracking.height, 
      Data.Tracking.cols, Data.Tracking.rows, 
      Data.Tracking.depthThreshold
    );
    width._int = Data.Tracking.width;
    height._int = Data.Tracking.height;
    rows._int = Data.Tracking.rows;
    cols._int = Data.Tracking.cols;
    depthThreshold._float = Data.Tracking.depthThreshold;
    profileNear._float = Data.Tracking.profileNear;
    profileFar._float = Data.Tracking.profileFar;
    
    // camera
    cameraResolution.value = Data.Camera.resolutionIndex;    
    cameraPositionX._float = Data.Camera.position.x;
    cameraPositionY._float = Data.Camera.position.y;
    cameraPositionZ._float = Data.Camera.position.z;
    size._float = Data.Camera.size;    
    near._float = Data.Camera.near;    
    far._float = Data.Camera.far;    
    cameraSettingsController.LoadData(Data.Camera.cameraData);
  }

  public void SaveData() {

    // save on data class

    // network
    Data.Network.ip = ip._string;
    Data.Network.port = port._string;    

    // tracking
    Data.Tracking.width = width._int;
    Data.Tracking.height = height._int;
    Data.Tracking.rows = rows._int;
    Data.Tracking.cols = cols._int;
    Data.Tracking.depthThreshold = depthThreshold._float;
    Data.Tracking.profileNear = profileNear._float;
    Data.Tracking.profileFar = profileFar._float;

    // camera    
    Data.Camera.resolutionIndex = cameraResolution.value;
    Data.Camera.position = new Vector3(cameraPositionX._float, cameraPositionY._float, cameraPositionZ._float);
    Data.Camera.size = size._float;
    Data.Camera.near = near._float;
    Data.Camera.far = far._float;
    Data.Camera.cameraData = cameraSettingsController.GetCameraData();
    
    // save on json
    manager.SaveSettingsInJson();
  }
  #endregion


  #region Debug Event Listner
  void AddDebugEventListner() {
    depthThreshold.AddDebugEventListner(UpdateDebugValues);
    profileNear.AddDebugEventListner(UpdateDebugValues);
    profileFar.AddDebugEventListner(UpdateDebugValues);
    cameraPositionX.AddDebugEventListner(UpdateDebugValues);
    cameraPositionY.AddDebugEventListner(UpdateDebugValues);
    cameraPositionZ.AddDebugEventListner(UpdateDebugValues);
    size.AddDebugEventListner(UpdateDebugValues);
    near.AddDebugEventListner(UpdateDebugValues);
    far.AddDebugEventListner(UpdateDebugValues);
  }

  void UpdateDebugValues(string value) {
    
    trackingManager.depthThreshold = depthThreshold._float;
    trackingManager.rsColorizer.minDist = profileNear._float;
    trackingManager.rsColorizer.maxDist = profileFar._float;
    trackingManager.trackingCamera.transform.position = new Vector3(cameraPositionX._float, cameraPositionY._float, cameraPositionZ._float);
    trackingManager.trackingCamera.orthographicSize = size._float;
    trackingManager.trackingCamera.nearClipPlane = near._float;
    trackingManager.trackingCamera.farClipPlane = far._float;
  }  
  #endregion
}
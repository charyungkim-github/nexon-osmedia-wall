using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  #region Public Variables
  [Header("General")]
  public GameObject settingsHolder;
  public GameObject[] settings;
  public Image[] tabs;

  [Header("Network")]
  public Text networkStatus;
  public Text networkConnection;
  public InputField ip;
  public InputField port;

  [Header("Tracking")]
  public Text trackingStatus;
  public InputField width;
  public InputField height;
  public InputField rows;
  public InputField cols;
  public InputField depthThreshold;
  public Toggle debugTracking;

  [Header("Camera")]
  public CameraSettingsController cameraSettingsController;
  public Dropdown cameraResolution;
  public InputField cameraPositionX;
  public InputField cameraPositionY;
  public InputField cameraPositionZ;
  public InputField size;
  public InputField near;
  public InputField far;
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

    // reset
    CloseSetting();
  }

  void Update() {
    // check for network status
    if(isOnSetting) networkConnection.text = Data.Network.isConnected ? "ON" : "OFF";  
    
    // check for device status
    // ??  
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
    Data.Tracking.debug = debugTracking.isOn;
  }
  #endregion

  #region Save and Load Data
  public void LoadData() {

    // network
    networkStatus.text = string.Format("{0}, {1}", Data.Network.ip, Data.Network.port);
    ip.text = Data.Network.ip;
    port.text = Data.Network.port;

    // tracking
    trackingStatus.text = string.Format(
      "size : {0} x {1}, cells : {2} x {3}, thresold : {4}", 
      Data.Tracking.rows, Data.Tracking.cols, 
      Data.Tracking.width, Data.Tracking.height, 
      Data.Tracking.depthThreshold
    );
    width.text = Data.Tracking.width.ToString();
    height.text = Data.Tracking.height.ToString();
    rows.text = Data.Tracking.rows.ToString();
    cols.text = Data.Tracking.cols.ToString();
    depthThreshold.text = Data.Tracking.depthThreshold.ToString();
    
    // camera
    cameraResolution.value = Data.Camera.resolutionIndex;    
    cameraPositionX.text = Data.Camera.position.x.ToString();
    cameraPositionY.text = Data.Camera.position.y.ToString();
    cameraPositionZ.text = Data.Camera.position.z.ToString();
    size.text = Data.Camera.size.ToString();    
    near.text = Data.Camera.near.ToString();    
    far.text = Data.Camera.far.ToString();    
    cameraSettingsController.LoadData(Data.Camera.cameraData);
  }

  public void SaveData() {

    // save on data class

    // network
    Data.Network.ip = ip.text;
    Data.Network.port = port.text;    

    // tracking
    Data.Tracking.width = int.Parse(width.text);
    Data.Tracking.height = int.Parse(height.text);
    Data.Tracking.rows = int.Parse(rows.text);
    Data.Tracking.cols = int.Parse(cols.text);
    Data.Tracking.depthThreshold = float.Parse(depthThreshold.text);

    // camera    
    Data.Camera.resolutionIndex = cameraResolution.value;
    Data.Camera.position = new Vector3(float.Parse(cameraPositionX.text), float.Parse(cameraPositionY.text), float.Parse(cameraPositionZ.text));
    Data.Camera.size = float.Parse(size.text);
    Data.Camera.near = float.Parse(near.text);
    Data.Camera.far = float.Parse(far.text);
    Data.Camera.cameraData = cameraSettingsController.GetCameraData();
    
    // save on json
    manager.SaveSettingsInJson();
  }
  #endregion
}
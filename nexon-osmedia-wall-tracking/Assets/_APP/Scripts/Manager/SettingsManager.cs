using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  /* Public Variables */ 
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

  /* Private Variables */ 
  Manager manager;
  bool isOnSetting = false;
  int activatedIndex = -1;

  /* Basic Functions */
  void Start() {

    // manager
    manager = GetComponentInParent<Manager>();

    // reset
    CloseSetting();
  }

  void Update() {
    // check for network status
    if(isOnSetting) networkConnection.text = NetworkInfo.isConnected ? "ON" : "OFF";    
  }

  /* Activate Settings */
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
    TrackingInfo.debug = debugTracking.isOn;
  }

  /* Save and Load Data */
  public void LoadData() {

    // network
    networkStatus.text = string.Format("{0}, {1}", NetworkInfo.ip, NetworkInfo.port);
    ip.text = NetworkInfo.ip;
    port.text = NetworkInfo.port;

    // tracking
    trackingStatus.text = string.Format(
      "size : {0} x {1}, cells : {2} x {3}, thresold : {4}", 
      TrackingInfo.rows, TrackingInfo.cols, 
      TrackingInfo.width, TrackingInfo.height, 
      TrackingInfo.depthThreshold
    );
    width.text = TrackingInfo.width.ToString();
    height.text = TrackingInfo.height.ToString();
    rows.text = TrackingInfo.rows.ToString();
    cols.text = TrackingInfo.cols.ToString();
    depthThreshold.text = TrackingInfo.depthThreshold.ToString();
  }

  public void SaveData() {

    // save on data class
    NetworkInfo.ip = ip.text;
    NetworkInfo.port = port.text;    
    TrackingInfo.width = int.Parse(width.text);
    TrackingInfo.height = int.Parse(height.text);
    TrackingInfo.rows = int.Parse(rows.text);
    TrackingInfo.cols = int.Parse(cols.text);
    TrackingInfo.depthThreshold = float.Parse(depthThreshold.text);

    // save on json
    manager.SaveSettingsInJson();
  }
}

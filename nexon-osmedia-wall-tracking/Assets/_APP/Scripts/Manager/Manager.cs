using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
  public bool debugDevice = false;
  public bool debugTracking = false;
  public bool debugGUI = true;
  DataManager dataManager;
  SettingsManager settingsManager;
  NetworkManager networkManager;
  TrackingManager trackingManager;

  bool isOnSetting = false;
  void Awake() {
    dataManager = GetComponentInChildren<DataManager>();
    settingsManager = GetComponentInChildren<SettingsManager>();
    networkManager = GetComponentInChildren<NetworkManager>();
    trackingManager = GetComponentInChildren<TrackingManager>();    
    
    dataManager.LoadJson();
  }

  void Start() {
  }

  void Update()
  {
    if(Input.GetKeyUp(KeyCode.D)) {
      ToggleSettingActive();
    }
    if(Input.GetKeyUp(KeyCode.M)) {
      debugTracking = !debugTracking;
    }
    if(Input.GetKeyUp(KeyCode.Q)) {
      Application.Quit();
    }
  }

  public void ToggleSettingActive() {
    isOnSetting = !isOnSetting;
    settingsManager.ToggleActive(isOnSetting);
    trackingManager.ToggleActive(isOnSetting);
  }

  public void SendTrackingData(List<bool> resultData) {
    networkManager.SendTrackingData(resultData);
  }

  public void SaveSettingsInJson() {

    // toggle setting
    ToggleSettingActive();

    // save json
    dataManager.SaveJson();

    // reset
    networkManager.Reset();
    trackingManager.Reset();
  }

  #region Debug
  public void ChangeDebugGUI(bool _debugGUI) {
    debugGUI = _debugGUI;
  }
  #endregion
}

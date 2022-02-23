﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
  DataManager dataManager;
  SettingsManager settingsManager;
  NetworkManager networkManager;
  TrackingManager trackingManager;

  bool isOnSetting = false;
  void Awake() {
    dataManager = GetComponentInChildren<DataManager>();
    dataManager.LoadJson();
  }

  void Start()
  {
    settingsManager = GetComponentInChildren<SettingsManager>();
    networkManager = GetComponentInChildren<NetworkManager>();
    trackingManager = GetComponentInChildren<TrackingManager>();
    settingsManager.Initialize();
    networkManager.Initialize();
    trackingManager.Initialize();
  }

  void Update()
  {
    if(Input.GetKeyUp(KeyCode.Space)) {
      ToggleSettingActive();
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

    // stop networking
    networkManager.Reset() ;

    // save json
    dataManager.SaveJson();
  }
}

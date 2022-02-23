using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  public GameObject settingsHolder;
  public GameObject[] settings;

  [Header("Network")]
  public Text networkStatus;
  public Text networkConnection;
  public InputField ip;
  public InputField port;


  Manager manager;
  GameObject activatedSetting;
  bool isOnSetting = false;
  bool isInitialize = false;

  /* Basic Functions */
  void Start() {    
  }

  public void Initialize() {    
    manager = GetComponentInParent<Manager>();

    CloseSettings();
    isInitialize = true;
  }

  public void ToggleActive(bool _isOnSetting) {
    isOnSetting = _isOnSetting;
    if(isOnSetting) {
      LoadData();
      OpenSettings(settings[0]);
    }
    else {
      CloseSettings();
    }
  }

  void Update() {
    if(!isInitialize) return;

    // check for network status
    if(isOnSetting) networkConnection.text = NetworkInfo.isConnected ? "ON" : "OFF";
  }

  /* Activate Settings */
  public void OpenSettings(GameObject target) {

    if(target == activatedSetting) return;
    
    if(!settingsHolder.activeSelf) settingsHolder.SetActive(true);
    if(activatedSetting) activatedSetting.SetActive(false);
    activatedSetting = target;
    activatedSetting.SetActive(true);
  }

  void CloseSettings() {
    settingsHolder.SetActive(false);
    foreach(GameObject setting in settings) {
      setting.SetActive(false);
    }
    activatedSetting = null;
  }

  /* Save and Load Data */
  public void LoadData() {
    networkStatus.text = string.Format("{0}, {1}", NetworkInfo.ip, NetworkInfo.port);
    ip.text = NetworkInfo.ip;
    port.text = NetworkInfo.port;
  }

  public void SaveData() {

    // close 
    // CloseSettings();
    // manager.ToggleSettingActive();

    // save on data class
    NetworkInfo.ip = ip.text;
    NetworkInfo.port = port.text;

    // save on json
    manager.SaveSettingsInJson();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class DataManager : MonoBehaviour
{
  string jsonFilePath;
  string jsonFileName = "settings.json";

  void Start() {
  }

  void Update() {
  }

  public void LoadJson() {
    // file path
    jsonFilePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

    // load json 
    string json = File.ReadAllText(jsonFilePath);
    JSONNode root = JSON.Parse(json);

    // load network info
    JSONNode newtork = root["network"];
    NetworkInfo.ip = newtork["ip"];
    NetworkInfo.port = newtork["port"];
    
    Debug.Log("json file loaded");
  }

  public void SaveJson() {

    // load json
    string json = File.ReadAllText(jsonFilePath);
    JSONNode root = JSON.Parse(json);

    // save network info
    JSONNode newtork = root["network"];
    newtork["ip"] = NetworkInfo.ip;
    newtork["port"] = NetworkInfo.port;
    
    // save into file
    File.WriteAllText(jsonFilePath, root.ToString());

    Debug.Log("json file saved");
  }
}

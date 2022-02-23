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
    
    // load tracking info
    JSONNode tracking = root["tracking"];
    TrackingInfo.width = tracking["width"];
    TrackingInfo.height = tracking["height"];
    TrackingInfo.rows = tracking["rows"];
    TrackingInfo.cols = tracking["cols"];
    TrackingInfo.depthThreshold = tracking["depthThreshold"];

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

    // save tracking info
    JSONNode tracking = root["tracking"];
    tracking["width"] = TrackingInfo.width;
    tracking["height"] = TrackingInfo.height;
    tracking["rows"] = TrackingInfo.rows;
    tracking["cols"] = TrackingInfo.cols;
    tracking["depthThreshold"] = TrackingInfo.depthThreshold;
    
    // save into file
    File.WriteAllText(jsonFilePath, root.ToString());

    Debug.Log("json file saved");
  }
}

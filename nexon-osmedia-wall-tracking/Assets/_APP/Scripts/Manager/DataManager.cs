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
    Data.Network.ip = newtork["ip"];
    Data.Network.port = newtork["port"];
    
    // load tracking info
    JSONNode tracking = root["tracking"];
    Data.Tracking.width = tracking["width"];
    Data.Tracking.height = tracking["height"];
    Data.Tracking.rows = tracking["rows"];
    Data.Tracking.cols = tracking["cols"];
    Data.Tracking.depthThreshold = tracking["depthThreshold"];

    Debug.Log("json file loaded");
  }

  public void SaveJson() {

    // load json
    string json = File.ReadAllText(jsonFilePath);
    JSONNode root = JSON.Parse(json);

    // save network info
    JSONNode newtork = root["network"];
    newtork["ip"] = Data.Network.ip;
    newtork["port"] = Data.Network.port;

    // save tracking info
    JSONNode tracking = root["tracking"];
    tracking["width"] = Data.Tracking.width;
    tracking["height"] = Data.Tracking.height;
    tracking["rows"] = Data.Tracking.rows;
    tracking["cols"] = Data.Tracking.cols;
    tracking["depthThreshold"] = Data.Tracking.depthThreshold;
    
    // save into file
    File.WriteAllText(jsonFilePath, root.ToString());

    Debug.Log("json file saved");
  }
}

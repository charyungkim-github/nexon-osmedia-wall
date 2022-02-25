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
    
    // load camera info
    JSONNode camera = root["camera"].AsArray;
    for(int i=0; i<camera.Count; i++) {
      CameraData cameraData = new CameraData();
      cameraData.serialNumber = camera[i]["serialNumber"];
      cameraData.order = camera[i]["order"];
      cameraData.width = camera[i]["width"];
      cameraData.height = camera[i]["height"];
      Data.Camera.Add(cameraData);
    }

    Debug.Log("json file loaded");
  }

  public void SaveJson() {
    JSONObject json = new JSONObject();

    // save network info
    JSONObject network = new JSONObject();
    json.AddField("network", network);
    network.AddField("ip", Data.Network.ip);
    network.AddField("port", Data.Network.port);

    // save tracking info
    JSONObject tracking = new JSONObject();
    json.AddField("tracking", tracking);
    tracking.AddField("width", Data.Tracking.width);
    tracking.AddField("height", Data.Tracking.height);
    tracking.AddField("rows", Data.Tracking.rows);
    tracking.AddField("cols", Data.Tracking.cols);
    tracking.AddField("depthThreshold", Data.Tracking.depthThreshold);
    
    // save camera info    
    JSONObject cameras = new JSONObject(JSONObject.Type.ARRAY);    
    json.AddField("camera", cameras);
    for(int i=0; i<Data.Camera.Count; i++) {
      JSONObject camera = new JSONObject();
      camera.AddField("serialNumber", Data.Camera[i].serialNumber);
      camera.AddField("order", Data.Camera[i].order);
      camera.AddField("width", Data.Camera[i].width);
      camera.AddField("height", Data.Camera[i].height);
      cameras.Add(camera);
    }
    
    // save into file
    File.WriteAllText(jsonFilePath, json.ToString());

    Debug.Log("json file saved");
  }
}

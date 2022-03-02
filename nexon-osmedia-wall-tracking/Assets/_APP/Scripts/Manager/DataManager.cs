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
    JSONNode camera = root["camera"];
    Data.Camera.resolutionIndex = camera["resolutionIndex"];
    JSONNode cameraData = camera["cameraData"].AsArray;
    for(int i=0; i<camera.Count; i++) {
      CameraData _cameraData = new CameraData();
      _cameraData.serialNumber = cameraData[i]["serialNumber"];
      _cameraData.width = cameraData[i]["width"];
      _cameraData.height = cameraData[i]["height"];
      _cameraData.positionX = cameraData[i]["positionX"];
      _cameraData.positionY = cameraData[i]["positionY"];
      _cameraData.rotationZ = cameraData[i]["rotationZ"];
      Data.Camera.cameraData.Add(_cameraData);
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
    JSONObject camera = new JSONObject();
    json.AddField("camera", camera);
    camera.AddField("resolutionIndex", Data.Camera.resolutionIndex);
    JSONObject cameraData = new JSONObject(JSONObject.Type.ARRAY);
    camera.AddField("cameraData", cameraData);
    for(int i=0; i<Data.Camera.cameraData.Count; i++) {
      JSONObject _cameraData = new JSONObject();
      _cameraData.AddField("serialNumber", Data.Camera.cameraData[i].serialNumber);
      _cameraData.AddField("width", Data.Camera.cameraData[i].width);
      _cameraData.AddField("height", Data.Camera.cameraData[i].height);
      _cameraData.AddField("positionX", Data.Camera.cameraData[i].positionX);
      _cameraData.AddField("positionY", Data.Camera.cameraData[i].positionY);
      _cameraData.AddField("rotationZ", Data.Camera.cameraData[i].rotationZ);
      cameraData.Add(_cameraData);
    }
    
    // save into file
    File.WriteAllText(jsonFilePath, json.ToString());

    Debug.Log("json file saved");
  }
}

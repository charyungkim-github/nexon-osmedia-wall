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
    Data.Tracking.profileNear = tracking["profileNear"];
    Data.Tracking.profileFar = tracking["profileFar"];
    
    // load camera info
    JSONNode camera = root["camera"];
    Data.Camera.resolutionIndex = camera["resolutionIndex"];
    Data.Camera.position.x = camera["positionX"];
    Data.Camera.position.y = camera["positionY"];
    Data.Camera.position.z = camera["positionZ"];
    Data.Camera.size = camera["size"];
    Data.Camera.near = camera["near"];
    Data.Camera.far = camera["far"];
    JSONNode cameraData = camera["cameraData"].AsArray;
    for(int i=0; i<cameraData.Count; i++) {
      CameraData _cameraData = new CameraData();
      _cameraData.serialNumber = cameraData[i]["serialNumber"];
      _cameraData.position = new Vector3(cameraData[i]["positionX"], cameraData[i]["positionY"], cameraData[i]["positionZ"]);
      _cameraData.rotation = new Vector3(cameraData[i]["rotationX"], cameraData[i]["rotationY"], cameraData[i]["rotationZ"]);
      _cameraData.scale = new Vector3(cameraData[i]["scaleX"], cameraData[i]["scaleY"], cameraData[i]["scaleZ"]);
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
    tracking.AddField("profileNear", Data.Tracking.profileNear);
    tracking.AddField("profileFar", Data.Tracking.profileFar);
    
    // save camera info    
    JSONObject camera = new JSONObject();
    json.AddField("camera", camera);
    camera.AddField("resolutionIndex", Data.Camera.resolutionIndex);
    camera.AddField("positionX", Data.Camera.position.x);
    camera.AddField("positionY", Data.Camera.position.y);
    camera.AddField("positionZ", Data.Camera.position.z);
    camera.AddField("size", Data.Camera.size);
    camera.AddField("near", Data.Camera.near);
    camera.AddField("far", Data.Camera.far);
    JSONObject cameraData = new JSONObject(JSONObject.Type.ARRAY);
    camera.AddField("cameraData", cameraData);
    for(int i=0; i<Data.Camera.cameraData.Count; i++) {
      JSONObject _cameraData = new JSONObject();
      _cameraData.AddField("serialNumber", Data.Camera.cameraData[i].serialNumber);
      _cameraData.AddField("positionX", Data.Camera.cameraData[i].position.x);
      _cameraData.AddField("positionY", Data.Camera.cameraData[i].position.y);
      _cameraData.AddField("positionZ", Data.Camera.cameraData[i].position.z);
      _cameraData.AddField("rotationX", Data.Camera.cameraData[i].rotation.x);
      _cameraData.AddField("rotationY", Data.Camera.cameraData[i].rotation.y);
      _cameraData.AddField("rotationZ", Data.Camera.cameraData[i].rotation.z);
      _cameraData.AddField("scaleX", Data.Camera.cameraData[i].scale.x);
      _cameraData.AddField("scaleY", Data.Camera.cameraData[i].scale.y);
      _cameraData.AddField("scaleZ", Data.Camera.cameraData[i].scale.z);      
      cameraData.Add(_cameraData);
    }
    
    // save into file
    File.WriteAllText(jsonFilePath, json.ToString());

    Debug.Log("json file saved");
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
  public bool debugServer = false;

  // manager
  Manager manager;
  
  // socket
  SocketIOComponent socket;
  bool isConnected = false;
  bool isInitialize = false;

  public void Initialize() {
    isInitialize = true;

    // manager
    manager = GetComponentInParent<Manager>();

    // connect socket
    socket = GetComponent<SocketIOComponent>();
    socket.url = string.Format("ws://{0}:{1}/socket.io/?EIO=4&transport=websocket", NetworkInfo.ip, NetworkInfo.port);    
    socket.Connect();

    // receive
    socket.On("socket-connected", GetSocketStatus);
  }

  void Start() { 

  }

  public void GetSocketStatus(SocketIOEvent  e) {
    
    isConnected = true;
    NetworkInfo.isConnected = true;
    Debug.Log("socket connected");
  }

  public void SendTrackingData(List<bool> resultData) {
    if(!isInitialize) return;
    if(debugServer) return;
    if(!isConnected) return;
    if(resultData.Count < 1) return;

    JSONObject json = new JSONObject();
    JSONObject jsonArray = new JSONObject(JSONObject.Type.ARRAY);
    json.AddField("activatedIndexList", jsonArray);
    for(int i=0; i<resultData.Count; i++) {
      if(resultData[i]) jsonArray.Add(i);
    }

    socket.Emit("tracking-data", json);
    // Debug.Log("send : " + json);
  }

  void Update() { }

  public void Close() {
    
    socket.Close();
    Debug.Log("socket disconnected");
  }

  public void Reset() {
    NetworkInfo.isConnected = false;
    socket.url = string.Format("ws://{0}:{1}/socket.io/?EIO=4&transport=websocket", NetworkInfo.ip, NetworkInfo.port);    
    socket.Connect();
  }
}

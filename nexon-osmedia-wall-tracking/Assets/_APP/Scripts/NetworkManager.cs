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

  void Start() {        

    // manager
    manager = GetComponentInParent<Manager>();

    // socket
    socket = GetComponent<SocketIOComponent>();

    // socket connect
    if(!debugServer) {
      socket.enabled = true;
      socket.Connect();
    }

    // receive
    socket.On("socket-connected", GetSocketStatus);
  }

  public void GetSocketStatus(SocketIOEvent  e) {
    
    isConnected = true;
    Debug.Log("socket connected");
  }

  public void SendTrackingData(List<bool> resultData) {

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

    Debug.Log("send : " + json);
  }

  void Update() { }

  public void Close() {
    socket.Close();
    Debug.Log("socket disconnected");
  }
}

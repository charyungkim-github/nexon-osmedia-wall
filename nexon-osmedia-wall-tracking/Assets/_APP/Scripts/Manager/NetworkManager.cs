using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
  public bool debugServer = false;
  Manager manager;
  SocketIOComponent socket;
  
  /* Basic Functions */
  void Start() { 

    // manager
    manager = GetComponentInParent<Manager>();
    socket = GetComponent<SocketIOComponent>();

    // reset
    Reset();

    // socket receive
    socket.On("socket-connected", GetSocketStatus);
  }

  void Update() {
  }

  /* Socket Connection */
  public void Reset() {
    Data.Network.isConnected = false;
    socket.url = string.Format("ws://{0}:{1}/socket.io/?EIO=4&transport=websocket", Data.Network.ip, Data.Network.port);    
    socket.Connect();
  }

  public void Close() {
    
    socket.Close();
    Debug.Log("socket disconnected");
  }  

  /* Socket Events */
  public void GetSocketStatus(SocketIOEvent  e) {
    
    Data.Network.isConnected = true;
    Debug.Log("socket connected");
  }

  public void SendTrackingData(List<bool> resultData) {
    
    if(debugServer) return;
    if(!socket.IsConnected) return;
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
}

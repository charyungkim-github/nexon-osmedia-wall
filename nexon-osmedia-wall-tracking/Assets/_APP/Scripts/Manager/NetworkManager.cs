using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
  public bool debugServer = false;
  Manager manager;
  SocketIOComponent socket;
  

  #region Basic Functions
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
  #endregion


  #region Socket Connection
  public void Reset() {
    Data.Network.isConnected = false;
    socket.url = string.Format("ws://{0}:{1}/socket.io/?EIO=4&transport=websocket", Data.Network.ip, Data.Network.port);    
    socket.Connect();
  }

  public void Close() {
    
    socket.Close();
    Debug.Log("socket disconnected");
  }  
  #endregion


  #region Socket Events
  public void GetSocketStatus(SocketIOEvent  e) {
    
    Data.Network.isConnected = true;
    Debug.Log("socket connected");
  }

  // public void SendTrackingData(List<bool> resultData) {
    
  //   if(debugServer) return;
  //   if(!socket.IsConnected) return;
  //   if(resultData.Count < 1) return;

  //   JSONObject json = new JSONObject();
  //   JSONObject jsonArray = new JSONObject(JSONObject.Type.ARRAY);
  //   json.AddField("activatedIndexList", jsonArray);
  //   for(int i=0; i<resultData.Count; i++) {
  //     if(resultData[i]) jsonArray.Add(i);
  //   }

  //   socket.Emit("tracking-data", json);
  //   // Debug.Log("send : " + json);
  // }  
  public void SendTrackingData(List<bool> indexData, List<float> valueData) {
    
    if(debugServer) return;
    if(!socket.IsConnected) return;
    if(indexData.Count < 1) return;
    
    // json object
    JSONObject json = new JSONObject();

    // index data
    JSONObject indexJsonArray = new JSONObject(JSONObject.Type.ARRAY);
    json.AddField("indexData", indexJsonArray);
    for(int i=0; i<indexData.Count; i++) {
      if(indexData[i]) indexJsonArray.Add(i);
    }

    // value data    
    JSONObject valueJsonArray = new JSONObject(JSONObject.Type.ARRAY);
    json.AddField("valueData", valueJsonArray);
    for(int i=0; i<valueData.Count; i++) {
      valueJsonArray.Add(valueData[i]);
    }

    // emit
    socket.Emit("tracking-data", json);

    // Debug.Log("send : " + json);
  }  
  #endregion
}

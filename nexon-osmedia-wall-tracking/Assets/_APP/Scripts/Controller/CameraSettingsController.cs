using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsController : MonoBehaviour
{
  public Transform depthCameraHolder;
  public Transform depthCameraSettingHolder;
  public GameObject depthCameraSettingPrefab;

  void Start() {
      
  }

  void Update() {
  }

  public void UpdateDepthCameraStatus() {
    if(depthCameraHolder.childCount > 0) {
      for(int i=0; i<depthCameraSettingHolder.childCount; i++) {
        if(depthCameraHolder.childCount <= i || depthCameraSettingHolder.childCount <= i) continue;
        depthCameraSettingHolder.GetChild(i).GetChild(1).GetComponent<Text>().text = depthCameraHolder.GetChild(i).GetComponent<RsDevice>().Streaming.ToString();
      }
    }
  }

  public void AddCameraSetting() {
    GameObject cameraSetting = Instantiate(depthCameraSettingPrefab);
    cameraSetting.transform.SetParent(depthCameraSettingHolder);

    // delete button onclick event
    cameraSetting.transform.GetChild(2).GetComponent<Button>().onClick.AddListener( () => { Destroy(cameraSetting); });

    // debug event
    AddDebugEventListner(cameraSetting.transform);
  }

  public List<CameraData> GetCameraData() {
    List<CameraData> cameras = new List<CameraData>();
    for(int i=0; i<depthCameraSettingHolder.childCount; i++) {
      Transform cameraSetting = depthCameraSettingHolder.GetChild(i);
      CameraData camera = new CameraData();
      camera.serialNumber = cameraSetting.GetChild(4).GetComponent<InputfieldController>()._string;
      camera.position.x = cameraSetting.GetChild(6).GetComponent<InputfieldController>()._float;
      camera.position.y = cameraSetting.GetChild(7).GetComponent<InputfieldController>()._float;
      camera.position.z = cameraSetting.GetChild(8).GetComponent<InputfieldController>()._float;
      camera.rotation.x = cameraSetting.GetChild(10).GetComponent<InputfieldController>()._float;
      camera.rotation.y = cameraSetting.GetChild(11).GetComponent<InputfieldController>()._float;
      camera.rotation.z = cameraSetting.GetChild(12).GetComponent<InputfieldController>()._float;
      camera.scale.x = cameraSetting.GetChild(14).GetComponent<InputfieldController>()._float;
      camera.scale.y = cameraSetting.GetChild(15).GetComponent<InputfieldController>()._float;
      camera.scale.z = cameraSetting.GetChild(16).GetComponent<InputfieldController>()._float;
      cameras.Add(camera);
    }
    return cameras;
  }

  public void LoadData(List<CameraData> cameras) {
    
    for(int i=0; i<cameras.Count; i++) {
      
      // create camera prefab
      if(i >= depthCameraSettingHolder.childCount) AddCameraSetting();
      
      // setup values
      Transform cameraSetting = depthCameraSettingHolder.GetChild(i);
      cameraSetting.GetChild(4).GetComponent<InputfieldController>()._string = cameras[i].serialNumber;
      cameraSetting.GetChild(6).GetComponent<InputfieldController>()._float = cameras[i].position.x;
      cameraSetting.GetChild(7).GetComponent<InputfieldController>()._float = cameras[i].position.y;
      cameraSetting.GetChild(8).GetComponent<InputfieldController>()._float = cameras[i].position.z;
      cameraSetting.GetChild(10).GetComponent<InputfieldController>()._float = cameras[i].rotation.x;
      cameraSetting.GetChild(11).GetComponent<InputfieldController>()._float = cameras[i].rotation.y;
      cameraSetting.GetChild(12).GetComponent<InputfieldController>()._float = cameras[i].rotation.z;
      cameraSetting.GetChild(14).GetComponent<InputfieldController>()._float = cameras[i].scale.x;
      cameraSetting.GetChild(15).GetComponent<InputfieldController>()._float = cameras[i].scale.y;
      cameraSetting.GetChild(16).GetComponent<InputfieldController>()._float = cameras[i].scale.z;
    }
  }
  

  #region Debug Event Listner
  void AddDebugEventListner(Transform cameraSetting) {
    cameraSetting.GetChild(6).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(7).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(8).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(10).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(11).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(12).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(14).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(15).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
    cameraSetting.GetChild(16).GetComponent<InputfieldController>().AddDebugEventListner(UpdateDebugValues);
  }

  void UpdateDebugValues(string value) {

    for(int i = 0; i < depthCameraHolder.childCount; i++) {

      if(depthCameraHolder.childCount <= i || depthCameraSettingHolder.childCount <= i) continue;

      Transform targetDepthCamera = depthCameraHolder.GetChild(i);
      Transform targetCameraSetting = depthCameraSettingHolder.GetChild(i);

      targetDepthCamera.position = new Vector3(
        targetCameraSetting.GetChild(6).GetComponent<InputfieldController>()._float,
        targetCameraSetting.GetChild(7).GetComponent<InputfieldController>()._float,
        targetCameraSetting.GetChild(8).GetComponent<InputfieldController>()._float
      );
      targetDepthCamera.eulerAngles = new Vector3(
        targetCameraSetting.GetChild(10).GetComponent<InputfieldController>()._float,
        targetCameraSetting.GetChild(11).GetComponent<InputfieldController>()._float,
        targetCameraSetting.GetChild(12).GetComponent<InputfieldController>()._float
      );
      targetDepthCamera.localScale = new Vector3(
        targetCameraSetting.GetChild(14).GetComponent<InputfieldController>()._float,
        targetCameraSetting.GetChild(15).GetComponent<InputfieldController>()._float,
        targetCameraSetting.GetChild(16).GetComponent<InputfieldController>()._float
      );
    }
  }
  #endregion
}
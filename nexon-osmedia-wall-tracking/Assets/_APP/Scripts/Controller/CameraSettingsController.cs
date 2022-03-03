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
      for(int i=0; i<depthCameraSettingHolder.childCount; i++)
        depthCameraSettingHolder.GetChild(i).GetChild(1).GetComponent<Text>().text = depthCameraHolder.GetChild(i).GetComponent<RsDevice>().Streaming.ToString();
    }
  }

  public void AddCameraSetting() {
    GameObject cameraSetting = Instantiate(depthCameraSettingPrefab);
    cameraSetting.transform.SetParent(depthCameraSettingHolder);

    // delete button onclick event
    cameraSetting.transform.GetChild(2).GetComponent<Button>().onClick.AddListener( () => { Destroy(cameraSetting); });
  }

  public List<CameraData> GetCameraData() {
    List<CameraData> cameras = new List<CameraData>();
    for(int i=0; i<depthCameraSettingHolder.childCount; i++) {
      Transform cameraSetting = depthCameraSettingHolder.GetChild(i);
      CameraData camera = new CameraData();
      camera.serialNumber = cameraSetting.GetChild(4).GetComponent<InputField>().text;
      camera.position.x = float.Parse(cameraSetting.GetChild(6).GetComponent<InputField>().text);
      camera.position.y = float.Parse(cameraSetting.GetChild(7).GetComponent<InputField>().text);
      camera.position.z = float.Parse(cameraSetting.GetChild(8).GetComponent<InputField>().text);
      camera.rotation.x = float.Parse(cameraSetting.GetChild(10).GetComponent<InputField>().text);
      camera.rotation.y = float.Parse(cameraSetting.GetChild(11).GetComponent<InputField>().text);
      camera.rotation.z = float.Parse(cameraSetting.GetChild(12).GetComponent<InputField>().text);
      camera.scale.x = float.Parse(cameraSetting.GetChild(14).GetComponent<InputField>().text);
      camera.scale.y = float.Parse(cameraSetting.GetChild(15).GetComponent<InputField>().text);
      camera.scale.z = float.Parse(cameraSetting.GetChild(16).GetComponent<InputField>().text);
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
      cameraSetting.GetChild(4).GetComponent<InputField>().text = cameras[i].serialNumber;
      cameraSetting.GetChild(6).GetComponent<InputField>().text = cameras[i].position.x.ToString();
      cameraSetting.GetChild(7).GetComponent<InputField>().text = cameras[i].position.y.ToString();
      cameraSetting.GetChild(8).GetComponent<InputField>().text = cameras[i].position.z.ToString();
      cameraSetting.GetChild(10).GetComponent<InputField>().text = cameras[i].rotation.x.ToString();
      cameraSetting.GetChild(11).GetComponent<InputField>().text = cameras[i].rotation.y.ToString();
      cameraSetting.GetChild(12).GetComponent<InputField>().text = cameras[i].rotation.z.ToString();
      cameraSetting.GetChild(14).GetComponent<InputField>().text = cameras[i].scale.x.ToString();
      cameraSetting.GetChild(15).GetComponent<InputField>().text = cameras[i].scale.y.ToString();
      cameraSetting.GetChild(16).GetComponent<InputField>().text = cameras[i].scale.z.ToString();
    }
  }
}
